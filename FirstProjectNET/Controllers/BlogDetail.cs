using Microsoft.AspNetCore.Mvc;

namespace FirstProjectNET.Controllers
{
    public class BlogDetail : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
