using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using StreetFood.Models;
using StreetFood.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StreetFood.Controllers
{
    public class ProductManagementController:Controller
    {
        private readonly IProductRepository productManage;
        public readonly ICategoryRepository CategoryManagement;
        public readonly IWebHostEnvironment WebHostEnvironment;

        public ProductManagementController(IProductRepository productManage,
                                ICategoryRepository categoryManagement,
                                IWebHostEnvironment webHostEnvironment)
        {
            this.productManage = productManage;
            CategoryManagement = categoryManagement;
            WebHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var products = productManage.Gets().ToList();
            ViewBag.Categories = GetCategories();
            return View(products);
        }
        [HttpGet]
        public ViewResult Create()
        {
            ViewBag.Categories = GetCategories();
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
                    Address=model.Address,
                    Title = model.Title,
                    KeySearch = $"{model.Name.ToLower()} {model.Title.ToLower()}"
                };
                var fileName = string.Empty;
                if (model.Img != null)
                {
                    string uploadFolder = Path.Combine(WebHostEnvironment.WebRootPath, "Images/Product");
                    fileName = $"{Guid.NewGuid()}_{model.Img.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.Img.CopyTo(fs);
                    }
                }
                else
                {
                    string uploadFolder = Path.Combine(WebHostEnvironment.WebRootPath, "Images/Product");
                    fileName = $"{Guid.NewGuid()}_amthuchue.jpg";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.Img.CopyTo(fs);
                    }
                }
                product.Img = fileName;
                var newEmp = productManage.Create(product);
                ViewBag.Categories = GetCategories();
                return RedirectToAction("Index", "productmanagement");
            }
            ViewBag.Categories = GetCategories();
            return View();
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = productManage.Get(id);
            if (product == null)
            {
                ViewBag.Id = id;
                return View("~/Views/Error/ProductNotFound.cshtml");
            }
            if (product != null)
            {
                var edit = new HomeEditViewModel()
                {
                    Id = id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    AvatarPath = product.Img,
                    Title = product.Title,
                    CategoryId = product.CategoryId
                };
                ViewBag.Categories = GetCategories();
                return View(edit);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit(HomeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    Img = model.AvatarPath,
                    CategoryId = model.CategoryId,
                    Title = model.Title,
                    KeySearch = $"{model.Name.ToLower()} {model.Title.ToLower()}"
                };
                var fileName = string.Empty;
                if (model.Img != null)
                {
                    string uploadFolder = Path.Combine(WebHostEnvironment.WebRootPath, "Images/Product");
                    fileName = $"{Guid.NewGuid()}_{model.Img.FileName}";
                    var filePath = Path.Combine(uploadFolder, fileName);
                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        model.Img.CopyTo(fs);
                    }
                    product.Img = fileName;
                    if (!string.IsNullOrEmpty(model.AvatarPath))
                    {
                        string delFile = Path.Combine(WebHostEnvironment.WebRootPath,
                                            "Images/Product", model.AvatarPath);
                        System.IO.File.Delete(delFile);
                    }
                }
                var editEmp = productManage.Edit(product);
                if (editEmp != null)
                {
                    return RedirectToAction("index", "productmanagement");
                }
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            var product = productManage.Get(id);
            if (product == null)
            {
                ViewBag.Id = id;
                return View("~/Views/Error/ProductNotFound.cshtml");
            }
            if (product != null)
            {
                productManage.Delete(id);
                string delFile = Path.Combine(WebHostEnvironment.WebRootPath,
                                           "Image/ImgProduct", product.Img);
                System.IO.File.Delete(delFile);
                ViewBag.Categories = GetCategories();
                return RedirectToAction("Index", "Productmanagement");
            }
            return View();
        }
        public IActionResult Products(int id)
        {
            var category = CategoryManagement.Get(id);
            if (category == null)
            {
                ViewBag.Id = id;
                return View("~/Views/Error/ProductNotFound.cshtml");
            }
            if (category != null)
            {
                var Products = (from pr in productManage.Gets() where pr.CategoryId == category.CategoryId select pr).ToList();
                ViewBag.Categories = GetCategories();
                ViewBag.CategoryName = category.CategoryName;
                return View(Products);
            }
            ViewBag.Categories = GetCategories();
            return View();
        }
        public List<Category> GetCategories()
        {
            var categories = CategoryManagement.Gets();
            return categories.Select(c => new Category()
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            }).ToList();
        }
    }
}
