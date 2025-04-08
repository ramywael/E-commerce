using E_Commerce510.Models;
using E_Commerce510.Models.ViewModel;
using E_Commerce510.Repositories.IRepositories;
using E_Commerce510.Repositories.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace E_Commerce510.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductRepository _productRepository;

        public CartController(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            this._cartRepository = cartRepository;
            this._userManager = userManager;
            this._productRepository = productRepository;
        }
        public IActionResult Index()
        {
            var user = _userManager.GetUserId(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            var cartItems = _cartRepository.Get(
               filter: e => e.ApplicationUserId == user,
               includes: [
                   e=>e.product,
                   e=>e.ApplicationUser
                   ]
                ).ToList();
            var cartVm = new CartVm()
            {
                Items = cartItems,
                TotalSum = cartItems.Sum(e => e.product.Price * e.Count)
            };
            return View(cartVm);
        }


        public IActionResult AddToCart(int productId, int count)
        {
            var user = _userManager.GetUserId(User);
            var product = _productRepository.GetOne(filter: e => e.Id == productId);
            var cartItem = _cartRepository.GetOne(
                filter:
                e => e.ApplicationUserId == user
                &&
                e.ProductId == productId,
                includes: [
                    e=>e.product,
                    ]
                );
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "Identity" });

            if (cartItem == null)
            {
                if (product == null)
                {
                    return RedirectToAction("NotFoundPage", "Home");
                }
                else
                {
                    if (product.Quntity >= count)
                    {
                        Cart cart = new Cart()
                        {
                            ApplicationUserId = user,
                            Count = count,
                            ProductId = productId,
                        };
                        _cartRepository.Create(cart);
                    }
                    else
                        return RedirectToAction("Details", "Home", new { productId = productId });
                }

            }
            else
            {
                if(product.Quntity >=cartItem.Count+count)
                cartItem.Count += count;
            }
            _cartRepository.Commit();

            return RedirectToAction("Index");
        }


        public IActionResult Increment(int productId)
        {
            var userApp = _userManager.GetUserId(User);
            if (userApp == null)
                return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });
            else
            {
                var cartItem = _cartRepository.GetOne(
                    filter:
                    e => e.ApplicationUserId == userApp && e.ProductId == productId,
                    includes: [
                        e=>e.product
                        ]
                    );
                if (cartItem == null)
                    return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });
                if (cartItem.product.Quntity > cartItem.Count)
                {
                    cartItem.Count++;
                }
                _cartRepository.Commit();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Decrement(int productId)
        {
            var userApp = _userManager.GetUserId(User);
            if (userApp == null)
                return RedirectToAction("NotFoundPage", "Home");
            else
            {
                var cartItem = _cartRepository.GetOne(
                    filter:
                    e => e.ApplicationUserId == userApp && e.ProductId == productId,
                    includes: [
                        e=>e.product
                        ]
                    );
                if (cartItem == null)
                    return RedirectToAction("NotFoundPage", "Home");
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                }
                _cartRepository.Commit();
            }
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int productId)
        {
            var userApp = _userManager.GetUserId(User);
            var cartItem = _cartRepository.GetOne(
                filter: e => e.ProductId == productId && e.ApplicationUserId == userApp
                );
            if (cartItem == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            _cartRepository.Delete(cartItem);
            _cartRepository.Commit();

            return RedirectToAction("Index");
        }


        public IActionResult Pay()
        {
            var userApp = _userManager.GetUserId(User);
            var cart = _cartRepository.Get(filter: e => e.ApplicationUserId == userApp, includes: [e => e.product]).ToList();
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Success",
                CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel",
            };
            foreach(var item in cart)
            {
                options.LineItems.Add(
                 new SessionLineItemOptions
                 {
                     PriceData = new SessionLineItemPriceDataOptions
                     {
                         Currency = "egp",
                         ProductData = new SessionLineItemPriceDataProductDataOptions
                         {
                             Name = item.product.Name,
                             Description = item.product.Description,
                         },
                         UnitAmount = (long)item.product.Price*100,
                     },
                     Quantity = item.Count,
                 }
               );
            }
           
            var service = new SessionService();
            var session = service.Create(options);
            return Redirect(session.Url);
        }
    }
}
