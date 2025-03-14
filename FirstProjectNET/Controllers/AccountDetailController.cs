using Microsoft.AspNetCore.Mvc;

namespace FirstProjectNET.Controllers
{
    public class AccountDetailController : Controller
    {
        public IActionResult PersonalDetail()
        {
            return View();
        }
    }
}
