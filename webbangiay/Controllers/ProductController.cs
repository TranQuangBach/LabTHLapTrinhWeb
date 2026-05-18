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
        private static ICategoryRepository _categoryRepository;
        private static IProductRepository _productRepository;
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
            if (ModelState.IsValid)
            {
                _productRepository.Add(product);
                _productRepository.SaveChanges();
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToAction(nameof(Index));
            }
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

        // ========== CÁC ACTION CHO CATEGORY ==========

        // GET: Danh sách danh mục
        public IActionResult Categories()
        {
            var categories = _categoryRepository.GetAllCategories();
            return View(categories);
        }

        // GET: Thêm danh mục
        public IActionResult CreateCategory()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCategory(Category category)
        {
            // Debug: Kiểm tra dữ liệu nhận được
            System.Diagnostics.Debug.WriteLine($"=== CREATE CATEGORY POST ===");
            System.Diagnostics.Debug.WriteLine($"Category Name: {category?.Name}");
            System.Diagnostics.Debug.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (category == null)
            {
                TempData["ErrorMessage"] = "Dữ liệu danh mục không hợp lệ!";
                return View(category);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra tên danh mục đã tồn tại chưa
                    var existingCategory = _categoryRepository.GetAllCategories()
                        .FirstOrDefault(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase));

                    if (existingCategory != null)
                    {
                        ModelState.AddModelError("Name", "Tên danh mục đã tồn tại!");
                        return View(category);
                    }

                    _categoryRepository.AddCategory(category);
                    _categoryRepository.SaveChanges();

                    TempData["SuccessMessage"] = $"Đã thêm danh mục \"{category.Name}\" thành công!";
                    return RedirectToAction(nameof(Categories));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi: {ex.Message}");
                    TempData["ErrorMessage"] = $"Lỗi: {ex.Message}";
                }
            }
            else
            {
                // Log lỗi validation
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }

            return View(category);
        }

        // GET: Sửa danh mục
        public IActionResult EditCategory(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _categoryRepository.UpdateCategory(category);
                _categoryRepository.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật danh mục thành công!";
                return RedirectToAction(nameof(Categories));
            }
            return View(category);
        }

        // GET: Xóa danh mục
        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategoryConfirmed(int id)
        {
            _categoryRepository.DeleteCategory(id);
            _categoryRepository.SaveChanges();
            TempData["SuccessMessage"] = "Xóa danh mục thành công!";
            return RedirectToAction(nameof(Categories));
        }

        // Lọc sản phẩm theo danh mục
        public IActionResult FilterByCategory(int categoryId)
        {
            // Lọc sản phẩm theo category
            var products = _productRepository.GetAll().Where(p => p.CategoryId == categoryId);

            // QUAN TRỌNG: Lấy danh sách categories để hiển thị bộ lọc
            var categories = _categoryRepository.GetAllCategories();

            // Truyền categories xuống View qua ViewBag
            ViewBag.Categories = categories ?? new List<Category>();
            ViewBag.SelectedCategoryId = categoryId;

            // Trả về View Index với products đã được lọc
            return View("Index", products);
        }

    }
}