using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin, SuperAdmin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
