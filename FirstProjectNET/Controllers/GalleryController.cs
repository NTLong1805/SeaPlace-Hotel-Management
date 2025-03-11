using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstProjectNET.Controllers
{
	   
    public class GalleryController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {	
			if (HttpContext.Session.GetString("Username") == null)
			{
				return RedirectToAction("Auth", "Authentication");
			}
			return View();
		}
    }
}
