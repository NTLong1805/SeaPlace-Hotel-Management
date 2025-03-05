using Microsoft.AspNetCore.Mvc;

namespace FirstProjectNET.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
