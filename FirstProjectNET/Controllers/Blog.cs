using Microsoft.AspNetCore.Mvc;

namespace FirstProjectNET.Controllers
{
    public class Blog : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
