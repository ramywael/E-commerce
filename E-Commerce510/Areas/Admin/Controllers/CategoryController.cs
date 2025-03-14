//using E_Commerce510.Data;
using E_Commerce510.Models;
//using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using E_Commerce510.Data;
using Microsoft.AspNetCore.Mvc;
using E_Commerce510.Repositories;
using E_Commerce510.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace E_Commerce510.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, SuperAdmin")]

    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository) {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = categoryRepository.Get();
            //
            return View(categories.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.Create(category);
                categoryRepository.Commit();
                 TempData["Notification"] = "Add Category Successfully";

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int categoryId)
        {
            var category = categoryRepository.GetOne(e=>e.Id == categoryId);
            if (category == null)
            {
                return RedirectToAction("NotFoundPage", "Home");
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category != null)
            {
                categoryRepository.Edit(category);
                categoryRepository.Commit();
                TempData["Notification"] = "Update Category Successfully";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("NotFoundPage", "Home");
        }

        public IActionResult Delete(int categoryId)
        {
            var category = categoryRepository.GetOne(e => e.Id == categoryId);

            if (category != null)
            {
                categoryRepository.Delete(category);
                categoryRepository.Commit();
                TempData["Notification"] = "Delete Category Successfully";

                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("NotFoundPage", "Home");

        }


    }
}
