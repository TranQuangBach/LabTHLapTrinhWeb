using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webbangiay.Models;
using webbangiay.Repository;

namespace webbangiay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderController(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderItemRepository = orderItemRepository;
        }

        // GET: Admin/Order
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepository.GetAllAsync();
            return View(orders);
        }

        // GET: Admin/Order/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // GET: Admin/Order/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Products = await _productRepository.GetAllAsync();
            ViewBag.OrderItems = new List<OrderItem>(); // Khởi tạo rỗng
            return View();
        }

        // POST: Admin/Order/Create
        // POST: Admin/Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, List<OrderItem> OrderItems) // Đổi tên parameter thành OrderItems
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
                return View(order);
            }
            // Xóa validation cho UserId vì admin tạo đơn không cần login
            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid && OrderItems != null && OrderItems.Any())
            {
                order.OrderDate = DateTime.Now;
                order.Status = "Pending";
                order.UserId = null; // Cho phép null vì admin tạo đơn offline
                order.TotalAmount = OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice);

                await _orderRepository.AddAsync(order);

                foreach (var item in OrderItems)
                {
                    item.OrderId = order.Id;
                    await _orderItemRepository.AddAsync(item);
                }

                TempData["Success"] = "Đơn hàng đã được tạo thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Log lỗi để debug
            if (OrderItems == null || !OrderItems.Any())
            {
                ModelState.AddModelError("", "Vui lòng thêm ít nhất một sản phẩm vào đơn hàng");
            }

            ViewBag.Products = await _productRepository.GetAllAsync();
            ViewBag.OrderItems = OrderItems ?? new List<OrderItem>();
            return View(order);
        }

        // GET: Admin/Order/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Admin/Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingOrder = await _orderRepository.GetByIdAsync(id);
                if (existingOrder == null)
                {
                    return NotFound();
                }

                existingOrder.Status = order.Status;
                existingOrder.ShippingAddress = order.ShippingAddress;
                existingOrder.PhoneNumber = order.PhoneNumber;
                existingOrder.RecipientName = order.RecipientName;
                existingOrder.Notes = order.Notes;

                await _orderRepository.UpdateAsync(existingOrder);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Admin/Order/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Xóa order items trước
            var orderItems = await _orderItemRepository.GetByOrderIdAsync(id);
            foreach (var item in orderItems)
            {
                await _orderItemRepository.DeleteAsync(item.Id);
            }

            // Sau đó xóa order
            await _orderRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Order/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _orderRepository.UpdateAsync(order);
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}