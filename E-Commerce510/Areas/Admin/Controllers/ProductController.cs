using E_Commerce510.Data;
using E_Commerce510.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]

    public class ProductController : Controller
    {
        ApplicationDbContext dbContext= new ApplicationDbContext();
        public IActionResult Index()
        {
            var products = dbContext.Products;
            return View(products.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            var category= dbContext.Categories;
            ViewBag.Category = category;
            return View(new Product());
        }
        [HttpPost]
        public IActionResult Create(Product product, IFormFile file)
        {
            ModelState.Remove("file");
            ModelState.Remove("Img");

            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {

                    // fileName
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //filePath
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot\\images", fileName);

                    //Copy Img to file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    // Save Img into database
                    product.Img = fileName;



                    dbContext.Products.Add(product);
                    dbContext.SaveChanges();
                    TempData["Notification"] = "Add Product Successfully";
                    return RedirectToAction(nameof(Index));
                }

            }
            var category = dbContext.Categories;
            ViewBag.Category = category;
            return View(product);
        }
        [HttpGet]
        public IActionResult Edit(int productId)
        {
            var product = dbContext.Products.FirstOrDefault(e=>e.Id==productId);
            var categories = dbContext.Categories;
            var productWithCategory = new
            {
                product,
                categories
            };
            return View(productWithCategory);
        }


        [HttpPost]
        public IActionResult Edit(Product product,IFormFile file)
        {

            var productInDb = dbContext.Products.AsNoTracking().FirstOrDefault(e => e.Id == product.Id);

            if (file != null && file.Length > 0)
            {

                // fileName
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                //filePath
                var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot\\images", fileName);

                var oldPath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot\\images", productInDb.Img);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                //Copy Img to file
                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                // Save Img into database
                product.Img = fileName;
            }
            else
            {
                product.Img = productInDb.Img;
            }
            if (product != null)
            {
                dbContext.Products.Update(product);
                dbContext.SaveChanges();
                TempData["Notification"] = "Update Product Successfully";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Delete(int productId)
        {
            var product = dbContext.Products.FirstOrDefault(e=>e.Id==productId);
            if(product != null)
            {
                var oldPath = Path.Combine(Directory.GetCurrentDirectory(),
             "wwwroot\\images", product.Img);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
                dbContext.Products.Remove(product);
                dbContext.SaveChanges();
                TempData["Notification"] = "Delete Product Successfully";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("NotFoundPage", "Home");

        }

    }
}
