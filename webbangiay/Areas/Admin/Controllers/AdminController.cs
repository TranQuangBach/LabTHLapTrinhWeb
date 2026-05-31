using Microsoft.AspNetCore.Mvc;

namespace webbangiay.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Product");
        }
    }
}
