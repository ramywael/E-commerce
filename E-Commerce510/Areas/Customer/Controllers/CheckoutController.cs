using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Sucess(int orderId)
        {
            return View();
        }
        public IActionResult Cancel()
        {
            return View();
        }
    }
}
