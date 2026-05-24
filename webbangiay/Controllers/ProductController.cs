using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using webbangiay.Models;
using webbangiay.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace webbangiay.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        public ProductController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        // GET: Hiển thị danh sách sản phẩm
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            var categories = _categoryRepository.GetAllCategories();

            ViewBag.Categories = categories ?? new List<Category>();
            ViewBag.SelectedCategoryId = null;

            return View(products);
        }

        // GET: Hiển thị chi tiết sản phẩm
        public IActionResult Details(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Hiển thị form thêm sản phẩm
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name");
            return View();
        }

        // POST: Xử lý thêm sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            // Bỏ qua validate các thuộc tính liên kết ảo hoặc không nhập trên form
            ModelState.Remove("Colors");
            ModelState.Remove("ImageUrl");
            ModelState.Remove("Category");

            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(product.ImageUrl))
                {
                    product.ImageUrl = "~/images/sneaker1.png"; // Ảnh mặc định để không bị lỗi hiển thị
                }

                _productRepository.Add(product);
                _productRepository.SaveChanges();

                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index)); // Chuyển hướng về trang danh sách
            }

            // Nếu bị lỗi validate, nạp lại danh mục cho Dropdown và trả về chính form đó để sửa
            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Hiển thị form sửa sản phẩm
        public IActionResult Edit(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Xử lý sửa sản phẩm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _productRepository.Update(product);
                    _productRepository.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                }
                catch (Exception)
                {
                    if (_productRepository.GetById(id) == null)
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAllCategories(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Hiển thị form xác nhận xóa
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _productRepository.GetById(id);
            if (product != null)
            {
                _productRepository.Delete(id);
                _productRepository.SaveChanges();
                TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}