using E_Commerce510.Models;
using E_Commerce510.Models.ViewModel;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderController(IOrderRepository orderRepository,IOrderItemRepository orderItemRepository)
        {
            this._orderRepository = orderRepository;
            this._orderItemRepository = orderItemRepository;
        }
        public IActionResult Index()
        {
            var orders = _orderRepository.Get(includes: [e=>e.ApplicationUser]);
            return View(orders);
        }

        public IActionResult Details(int orderId)
        {
            var order = _orderRepository.GetOne(filter: e => e.OrderId == orderId, includes: [e=>e.ApplicationUser]);
            List<OrderItem> orderItems = new List<OrderItem>();
            if(order != null)
            {
               orderItems = _orderItemRepository.Get(filter: e => e.OrderId == orderId, includes: [e=>e.Product]).ToList();
            }
            OrderWithItemsVM orderWithItemsVM = new OrderWithItemsVM()
            {
                Order=order,
                OrderItems=orderItems
            };

            return View(orderWithItemsVM);
        }

        public IActionResult Refund(int orderId)
        {
            var order = _orderRepository.GetOne(filter:e=>e.OrderId==orderId);
            if(order.PaymentStripeId != null && order.PaymentStatus==true)
            {
                RefundCreateOptions options = new RefundCreateOptions()
                {
                    Amount=(long)order.OrderTotal,
                    PaymentIntent=order.PaymentStripeId,
                    Reason=RefundReasons.RequestedByCustomer,
                };
                var service = new RefundService();
                var session = service.Create(options);

                order.PaymentStripeId = null;
                order.Status = enOrderStatus.Canceled;
                _orderRepository.Commit();

                return View();
            }
            return RedirectToAction("NotFoundPage", "Home", new {area="Customer" });
        }
    }
}
