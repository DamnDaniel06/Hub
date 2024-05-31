using Microsoft.AspNetCore.Mvc;

namespace Hub.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
