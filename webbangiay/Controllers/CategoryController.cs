using Microsoft.AspNetCore.Mvc;
using webbangiay.Models;
using webbangiay.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace webbangiay.Controllers
{
    [Authorize]
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

        // GET: Hiển thị chi tiết danh mục (Bất đồng bộ)
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id); // Sử dụng hàm lấy ID của bạn
            if (category == null)
            {
                return NotFound();
            }

            // Lấy danh sách sản phẩm theo CategoryId
            var products = await _productRepository.GetProductsByCategoryIdAsync(id);

            // Truyền tên danh mục ra View để làm Tiêu đề
            ViewBag.CategoryName = category.Name;

            return View(products);
        }
    }
}