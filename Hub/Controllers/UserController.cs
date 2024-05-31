using Microsoft.AspNetCore.Mvc;

namespace Hub.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
