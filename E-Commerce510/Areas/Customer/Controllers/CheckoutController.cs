using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace E_Commerce510.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CheckoutController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public CheckoutController(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Success(int orderId)
        {
            var order = _orderRepository.GetOne(filter:e=>e.OrderId==orderId);
            if (order != null && order.PaymentStatus==false)
            {
                var service = new SessionService();
                var session = service.Get(order.SessionId);

                order.PaymentStripeId = session.PaymentIntentId;
                order.PaymentStatus = true;
                order.Status = Models.enOrderStatus.InProgress;
                _orderRepository.Commit();
                return View();
            }
            return RedirectToAction("NotFoundPage","Home");
        }
        public IActionResult Cancel()
        {
            return View();
        }
    }
}
