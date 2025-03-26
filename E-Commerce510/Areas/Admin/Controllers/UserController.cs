using E_Commerce510.Models;
using E_Commerce510.Models.ViewModel;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserRepository userRepository, UserManager<ApplicationUser> userManager)
        {
            this._userRepository = userRepository;
            this._userManager = userManager;
        }
        public IActionResult Index(string query, int page = 1)
        {
            var users = _userRepository.GetUsersInRole(
                "Customer"
                );

            if (query != null)
            {
                users = _userRepository.GetUsersInRole(
                "Customer",
                filter: e => e.UserName.Contains(query)
                );
            }
            int totalUsers = users.Count();
            int pageSize = 2;
            int pageNumber = (int)Math.Ceiling(totalUsers / (double)pageSize);

            ViewBag.PageNumber = pageNumber;

            if (page > pageNumber || page <= 0)
                return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });

            users = users.Skip((page - 1) * pageSize).Take(pageSize);

            return View(users.ToList());
        }

        public async Task<IActionResult> Status(LockOutUserVm lockOutUserVm, int page = 1)
        {
            var user = _userRepository.GetOne(filter: e => e.Id.Equals(lockOutUserVm.UserId));
            if (user == null)
                return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });
            if (user.LockoutEnd.HasValue&&user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                user.LockoutEnd = null;
                
            }
            else
            {
                user.LockoutEnd = new DateTimeOffset(lockOutUserVm.DateTime);
            }
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index", new { page });
        }
    }
}
