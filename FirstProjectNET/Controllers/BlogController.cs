using Microsoft.AspNetCore.Mvc;

namespace FirstProjectNET.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
