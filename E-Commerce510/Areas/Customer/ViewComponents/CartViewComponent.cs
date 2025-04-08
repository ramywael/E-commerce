using E_Commerce510.Models;
using E_Commerce510.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce510.Areas.Customer.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartRepository _cartRepository;

        public CartViewComponent(UserManager<ApplicationUser> userManager,ICartRepository cartRepository)
        {
            this._userManager = userManager;
            this._cartRepository = cartRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userApp = _userManager.GetUserId(HttpContext.User);
            if(userApp != null)
            {
                var TotalItems = _cartRepository.Get(filter: e => e.ApplicationUserId == userApp).Count();
                return View(TotalItems);
            }
            return View(0);
        }
    }
}
