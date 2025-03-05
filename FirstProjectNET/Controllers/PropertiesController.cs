using Microsoft.AspNetCore.Mvc;

namespace FirstProjectNET.Controllers
{
    public class PropertiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
