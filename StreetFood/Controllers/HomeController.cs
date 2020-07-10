using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StreetFood.Models;
using StreetFood.ViewModel;

namespace StreetFood.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICategoryRepository categoryRepository;

        public IProductRepository ProductRepository { get; }

        public HomeController(IWebHostEnvironment webHostEnvironment,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            this.webHostEnvironment = webHostEnvironment;
            ProductRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            ViewBag.Categories = categoryRepository.Gets();
            var products = ProductRepository.Gets().ToList();
            return View(products);
        }
        [HttpGet]
        public ViewResult Create()
        {
            ViewBag.Categories = categoryRepository.Gets();
            return View();
        }

        [HttpPost]
        public IActionResult Create(HomeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    Address = model.Address,
                    Title=model.Title,
                    KeySearch = $"{model.Name.ToLower()} {model.Address.ToLower()}"

                };
                var fileName = string.Empty;
                if (model.Img != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images/Product");
                    fileName = $"{Guid.NewGuid()}_{model.Img.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.Img.CopyTo(fs);
                    }
                }
                else
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images/Product");
                    fileName = $"{Guid.NewGuid()}_avatarfood.jpg";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.Img.CopyTo(fs);
                    }
                }
                product.Img = fileName;
                var newEmp = ProductRepository.Create(product);
                ViewBag.Categories = categoryRepository.Gets();
                return RedirectToAction("Details", new { id = newEmp.Id });
            }
            ViewBag.Categories = categoryRepository.Gets();
            return View();
        }
        public ViewResult Edit(int id)
        {
            var product = ProductRepository.Get(id);
            if (product == null)
            {
                return View("~/Views/Error/EmployeeNotFound.cshtml", id);
            }
            var empEdit = new HomeEditViewModel()
            {
                AvatarPath = product.Img,
                Name = product.Name,
                Price=product.Price,
                CategoryId=product.CategoryId,
                Address = product.Address,
                Description = product.Description,
                Title=product.Title,
                Id = product.Id
            };
            ViewBag.Categories = categoryRepository.Gets();
            return View(empEdit);
        }

        [HttpPost]
        public IActionResult Edit(HomeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Name = model.Name,
                    Address = model.Address,
                    Description = model.Description,
                    CategoryId=model.CategoryId,
                    Price=model.Price,
                    Id = model.Id,
                    Img = model.AvatarPath,
                    Title=model.Title
                };
                var fileName = string.Empty;
                if (model.Img != null)
                {
                    string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images/Product");
                    fileName = $"{Guid.NewGuid()}_{model.Img.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.Img.CopyTo(fs);
                    }
                    product.Img = fileName;
                    if (!string.IsNullOrEmpty(model.AvatarPath))
                    {
                        string delFile = Path.Combine(webHostEnvironment.WebRootPath,
                                            "Images/Product", model.AvatarPath);
                        System.IO.File.Delete(delFile);
                    }
                }
                else
                {
                    fileName = model.AvatarPath;
                }
                product.Img = fileName;
                var editEmp = ProductRepository.Edit(product);
                if (editEmp != null)
                {
                    return RedirectToAction("Details", new { id = product.Id });
                }
            }

            return View();
        }

        public IActionResult Details(int id)
        {
            var pd = ProductRepository.Get(id);
            return View(pd);
        }
        public IActionResult Products(int id)
        {
            List<Product> products = (from p in ProductRepository.Gets() where p.CategoryId == id select p).ToList();
            ViewBag.Categories = categoryRepository.Gets();
            return View(products);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Delete(int id)
        {
            var product = ProductRepository.Get(id);
            if (product == null)
            {
                ViewBag.Id = id;
                return View("~/Views/Error/ProductNotFound.cshtml");
            }
            if (product != null)
            {
                ProductRepository.Delete(id);
                string delFile = Path.Combine(webHostEnvironment.WebRootPath,
                                           "Images/Product", product.Img);
                System.IO.File.Delete(delFile);
                ViewBag.Categories = categoryRepository.Gets();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
