using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace E_Commerce510.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CheckoutController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(IOrderRepository orderRepository,ICartRepository cartRepository,UserManager<ApplicationUser> userManager)
        {
            this._orderRepository = orderRepository;
            this._cartRepository = cartRepository;
            this._userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Success(int orderId)
        {
            var order = _orderRepository.GetOne(filter:e=>e.OrderId==orderId);
            var cart = _cartRepository.Get(filter:e=>e.ApplicationUserId==_userManager.GetUserId(User));
            if (order != null && order.PaymentStatus==false)
            {
                var service = new SessionService();
                var session = service.Get(order.SessionId);

                order.PaymentStripeId = session.PaymentIntentId;
                order.PaymentStatus = true;
                order.Status = enOrderStatus.InProgress;
                _cartRepository.DeleteRange(cart.ToList());
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
