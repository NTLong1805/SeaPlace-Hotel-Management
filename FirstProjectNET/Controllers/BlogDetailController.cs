using Microsoft.AspNetCore.Mvc;

namespace FirstProjectNET.Controllers
{
    public class BlogDetailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
