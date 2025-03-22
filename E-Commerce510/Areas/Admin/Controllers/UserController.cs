using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserRepository userRepository,UserManager<ApplicationUser> userManager)
        {
            this._userRepository = userRepository;
            this._userManager = userManager;
        }
        public IActionResult Index(string query)
        {
            var users = _userRepository.GetUsersInRole(
                "Customer", 
                filter: e => e.UserName.Contains(query)
                );
            return View(users);
        }
    }
}
