using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webbangiay.Models;
using webbangiay.Repository;

namespace webbangiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        // Inject ICategoryRepository giống hệt cấu trúc bên Product
        public CategoryController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        // GET: Hiển thị danh sách danh mục (Bất đồng bộ)
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var products = await _productRepository.GetAllAsync(); // cần inject IProductRepository

            ViewBag.ProductCounts = products
                .GroupBy(p => p.CategoryId)
                .ToDictionary(g => g.Key, g => g.Count());

            return View(categories);
        }

        // GET: Hiển thị form thêm mới danh mục
        public IActionResult CreateCategory()
        {
            return View(new Category());
        }

        // POST: Xử lý lưu danh mục mới vào cơ sở dữ liệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (category == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                await _categoryRepository.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Hiển thị form cập nhật thông tin danh mục
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Xử lý cập nhật thông tin danh mục
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int id, Category category)
        {
            if (id != category.Id) // Kiểm tra Id truyền vào (Lưu ý: Khớp với thuộc tính CategoryId trong Model của bạn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(id);
                if (existingCategory == null)
                {
                    return NotFound();
                }

                // Cập nhật thông tin tương tự bên Product
                existingCategory.Name = category.Name;
                // Nếu Model Category của bạn có thêm các thuộc tính khác (ví dụ Description...), hãy map thêm ở đây

                await _categoryRepository.UpdateAsync(existingCategory);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Hiển thị form xác nhận xóa danh mục
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Xử lý xóa danh mục (Bất đồng bộ)
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productsToDelete = await _productRepository.GetProductsByCategoryIdAsync(id);

            foreach (var product in productsToDelete)
            {
                await _productRepository.DeleteAsync(product.Id);
            }

            await _categoryRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}