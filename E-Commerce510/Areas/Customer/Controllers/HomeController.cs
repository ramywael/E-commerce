using System.Diagnostics;
using E_Commerce510.Data;
using E_Commerce510.Models;
using E_Commerce510.Repositories.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce510.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IProductRepository productRepository,ICategoryRepository categoryRepository)
        {
            _logger = logger;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._productRepository = productRepository;
            this._categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(int categoryId, string productName, int minPrice, int maxPrice, bool isHot)
        {

            var user= await _userManager.GetUserAsync(User);

            if(user!=null && user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Block", "Account", new {area="Identity" });
            }

            IQueryable<Product> products = _productRepository.Get(includes: [e=>e.Category]);



            if (productName != null)
            {
                //products = products.Where(e => e.Name.Contains(productName));
                products = _productRepository.Get(filter: e => e.Name.Contains(productName));

            }

            if (minPrice > 0)
            {
                //products = products.Where(e => e.Price > minPrice);
                products = _productRepository.Get(filter:e=>e.Price >= minPrice);

            }


            if (maxPrice > 0)
            {
                //products = products.Where(e => e.Price < maxPrice);
                products = _productRepository.Get(e=>e.Price < maxPrice);

            }

            if (isHot)
            {
                products = _productRepository.Get(e=>e.Discount >12);

            }

            if (categoryId != 0)
            {
                //products = products.Where(e => e.CategoryId == categoryId);
                products = _productRepository.Get(filter:e=>e.CategoryId==categoryId);


            }
            ViewBag.AllCategories = _categoryRepository.Get().ToList();
            ViewBag.CategoryId = categoryId;
            ViewBag.ProductName = productName;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.IsHot = isHot;
            return View(products.ToList());
        }

        //public IActionResult Details(int productId)
        //{
        //    var product = dbContext.Products.Include(e => e.Category).FirstOrDefault(e => e.Id == productId);



        //    if (product != null)
        //    {
        //        var productRelated = dbContext.Products.Skip(1).Take(5).Where(e => e.Price <= product.Price).ToList();
        //        var productRelatedName = dbContext.Products.Skip(1).Take(5).Where(e => e.Name.Substring(1,4) == product.Name.Substring(1,4)).ToList();

        //        var productAndRelatedProducts = new
        //        {
        //            Product = product,
        //            //ProductRelated = productRelatedName,
        //            ProductRelatedName=productRelatedName,
        //        };
        //        return View(productAndRelatedProducts);
        //    }

        //    return RedirectToAction("NotFoundPage");
        //}
        public IActionResult Details(int productId)
        {
            //var product = dbContext.Products.Include(e => e.Category).FirstOrDefault(e => e.Id == productId);
            var product = _productRepository.GetOne(includes: [e => e.Category], filter: e=>e.Id==productId);

            if (product != null)
            {
                //var productWithSameCategory = dbContext.Products.Where(e => e.CategoryId == product.CategoryId).Skip(productId).Take(4)
                //    .ToList();

                var productWithSameCategory = _productRepository.Get(filter:e=>e.CategoryId==product.CategoryId).Skip(productId).Take(4)
                    .ToList();
                ViewBag.productWithSameCategory = productWithSameCategory;
                ViewData["productWithSameCategory"] = productWithSameCategory;

                return View(product);
            }
            return RedirectToAction("NotFoundPage", "Home");
        }
        public IActionResult NotFoundPage()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
