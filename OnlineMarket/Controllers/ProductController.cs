﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.BLL.Service.Interfaces;
using OnlineMarket.BLL.ViewModels.Category;
using OnlineMarket.BLL.ViewModels.Product;
using OnlineMarket.Models;

namespace OnlineMarket.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBasketService _basketService;
        private readonly ISubCategoryService _subCategoryService;
        public ProductController(IProductService productService, ICategoryService categoryService, IBasketService basketService, ISubCategoryService subCategoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _basketService = basketService;
            _subCategoryService = subCategoryService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetProduct()
        {
            List<ProductVM> productVMList = new List<ProductVM>();
            var productList = await _productService.GetAsync().ToListAsync();
            if (productList == null)
            {
                return NotFound();
            }
            foreach (var product in productList)
            {
                ProductVM productVM = new ProductVM();
                productVM.ProductName = product.ProductName;
                productVM.ProductDescription = product.ProductDescription;
                productVM.Price = product.Price;
                productVM.ProductPhoto = product.ProductPhoto;
                productVM.SubCategoryId = product.SubCategoryId;
                productVM.Id = product.Id;
                productVM.Quantity = product.Quantity;
                productVMList.Add(productVM);
            }

            return View(productVMList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ProductVM productVM = new ProductVM();
            List<CategoryVM> categoryVMList = new List<CategoryVM>();
            var category = await _categoryService.GetAsync().Select(x => new { x.Id, x.CategoryName }).ToListAsync();
            foreach (var cat in category)
            {
                CategoryVM categoryVM = new CategoryVM();
                categoryVM.CategoryName = cat.CategoryName;
                categoryVM.Id = cat.Id;
                categoryVMList.Add(categoryVM);
                
            }
            this.ViewBag.Category = new SelectList(categoryVMList, "Id", "CategoryName");

            return View(productVM);
        }
        public async Task<List<SubCategoryVM>> SelectSubCategory(int? id)
        {
            List<SubCategoryVM>? subCategoryVMList = new List<SubCategoryVM>();
            var subCategory = await _subCategoryService.GetAsync().Where(x => x.CategoryId == id).Select(x => new { x.Id, x.CategoryName }).ToListAsync();
            foreach (var sub in subCategory)
            {
                SubCategoryVM subVM = new SubCategoryVM();
                subVM.Id = sub.Id;
                subVM.CategoryName = sub.CategoryName;
                subCategoryVMList.Add(subVM);
            }

            return subCategoryVMList;
             
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM productVM, byte[]? imageData)
        {
            ProductVM prod = new ProductVM();
            List<CategoryVM> categoryVMList = new List<CategoryVM>();
            var category = await _categoryService.GetAsync().Select(x => new { x.Id, x.CategoryName }).ToListAsync();
            foreach (var cat in category)
            {
                CategoryVM categoryVM = new CategoryVM();
                categoryVM.CategoryName = cat.CategoryName;
                categoryVM.Id = cat.Id;
                categoryVMList.Add(categoryVM);

            }
            this.ViewBag.Category = new SelectList(categoryVMList, "Id", "CategoryName");

            if (productVM.Price == default)
            {
                ModelState.AddModelError("Price", "укажите стомость");

            }
            if (productVM.Quantity == default)
            {
                ModelState.AddModelError("Quantity", "укажите количество");
            }
            if (ModelState.IsValid)
            {
                using (var binaryReader = new BinaryReader(productVM.ProductformFile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)productVM.ProductformFile.Length);
                }

                productVM.ProductPhoto = imageData;

                await _productService.CreateAsync(productVM);
                return RedirectToAction("Index", "Home");
            }

            return View(prod);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _productService.GetAsync().FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ProductVM productVM = new ProductVM();
            productVM.ProductName = product.ProductName;
            productVM.ProductDescription = product.ProductDescription;
            productVM.Price = product.Price;
            productVM.ProductPhoto = product.ProductPhoto;
            productVM.SubCategoryId = product.SubCategoryId;
            productVM.Id = product.Id;
            productVM.Quantity = product.Quantity;

            return View(productVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.Delete(id);
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> SearchProduct(string searchString, int? scrolled, int pageNumber = 1)
        {
            const int PageSize = 12;
            int recsCount = _productService.GetAsync().Where(x => x.ProductName.Contains(searchString)).Count();
            var pager = new Pager(recsCount, pageNumber, PageSize * 2);
            this.ViewBag.Pager = pager;
            var productVMList = new List<ProductVM>();
            int scrollPage = scrolled ?? 0;
            int itemsToSkip = (pageNumber - 1) * PageSize;

            var searchQ = _productService.GetAsync().Where(x => x.ProductName.Contains(searchString));
            if (searchQ.Count() == 0)
            {

                ViewData["search"] = $"Поиск: {searchString} - ничего не найдено";
                return View(productVMList);
            }
            else
            {
                ViewData["search"] = "Поиск: " + searchString;
            }
            if (scrollPage < 2 && pageNumber == 1)
            {

                itemsToSkip = scrollPage * PageSize;
                var search = await _productService.GetAsync().Where(x => x.ProductName.Contains(searchString)).OrderBy(x => x.Id).Skip(itemsToSkip).Take(PageSize).ToListAsync();

                foreach (var product in search)
                {
                    ProductVM productVM = new ProductVM();
                    productVM.ProductName = product.ProductName;
                    productVM.ProductDescription = product.ProductDescription;
                    productVM.Price = product.Price;
                    productVM.ProductPhoto = product.ProductPhoto;
                    productVM.SubCategoryId = product.SubCategoryId;
                    productVM.Id = product.Id;
                    productVM.Quantity = product.Quantity;
                    productVMList.Add(productVM);
                }

                return View(productVMList);
            }
            else if (scrollPage < 2 && pageNumber != 1)
            {

                scrollPage = scrolled == null ? 0 : PageSize;
                var i = pageNumber;


                itemsToSkip = scrollPage + 2 * PageSize * (i - 1);
                var search = await _productService.GetAsync().Where(x => x.ProductName.Contains(searchString)).OrderBy(x => x.Id).Skip(itemsToSkip).Take(PageSize).ToListAsync();

                foreach (var product in search)
                {
                    ProductVM productVM = new ProductVM();
                    productVM.ProductName = product.ProductName;
                    productVM.ProductDescription = product.ProductDescription;
                    productVM.Price = product.Price;
                    productVM.ProductPhoto = product.ProductPhoto;
                    productVM.SubCategoryId = product.SubCategoryId;
                    productVM.Id = product.Id;
                    productVM.Quantity = product.Quantity;
                    productVMList.Add(productVM);
                }

                return View(productVMList);
            }
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _productService.GetAsync().FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ProductVM productVM = new ProductVM();
            productVM.ProductName = product.ProductName;
            productVM.ProductDescription = product.ProductDescription;
            productVM.Price = product.Price;
            productVM.ProductPhoto = product.ProductPhoto;
            productVM.SubCategoryId = product.SubCategoryId;
            productVM.Id = product.Id;
            productVM.Quantity = product.Quantity;

            return View(productVM);
        }
    }
}
