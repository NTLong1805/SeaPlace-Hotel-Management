using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FirstProjectNET.Models.ViewModel;
using FirstProjectNET.Data;
using FirstProjectNET.Models;
using NuGet.Protocol;
using FirstProjectNET.Models.Common;
using System.Text;
using System.Security.Cryptography;

namespace FirstProjectNET.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly HotelDbContext _context;
        public AuthenticationController(HotelDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Auth()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult SignUp(AuthenticationViewModel model)
        {
            model.IsRegister = true;
            ModelState.Remove("SignInEmail");
            ModelState.Remove("SignInPassword");
            string ErrorMessage = "";
			if (ModelState.IsValid)
			{
				if (_context.Accounts.Any(u => u.Email == model.SignUpEmail))
				{
					ErrorMessage = "Email was existsed";
					return View("Auth", model);
				}
				string hashPassword = HashPassword(model.SignUpPassword);

				var newAccount = new Account
				{
					//AccountID = nextAccountID,
					Email = model.SignUpEmail,
					Password = hashPassword,
					Username = model.SignUpUserName,
					Type = Models.Common.AccountType.Customer,
					Active = true,
				};
				_context.Accounts.Add(newAccount);
				_context.SaveChanges();

				//ViewBag.ErrorMessage = "Successful Sign Up";
				HttpContext.Session.SetString("Username", model.SignUpUserName);
				return RedirectToAction("Index", "Home");
			}
			ViewBag.ErrorMessage = ErrorMessage;
			return View("Auth", model);			
		}

		private string HashPassword(string signUpPassword)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(signUpPassword));
				return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
			}
		}

		[HttpPost]
        public IActionResult SignIn(AuthenticationViewModel model)
        {
            model.IsRegister = false;
            ModelState.Remove("SignUpEmail");
            ModelState.Remove("SignUpUsername");
            ModelState.Remove("SignUpPassword");
            ModelState.Remove("SignUpConfirmPassword");
            string errorMessage = "";
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("Username") == null)
                {
                    var user = _context.Accounts.FirstOrDefault(u => u.Email == model.SignInEmail);                    
                    if (user == null)
                    {
						ViewBag.ErrorMessage = "Account does not exists";
                        return View("Auth",model);
                    }
                    string hashedPassword = HashPassword(model.SignInPassword);
                    if(user.Password != hashedPassword)
                    {
                        ViewBag.ErrorMessage = "Wrong Password";
                        return View("Auth", model);
                    }    
                    if(user.Password == hashedPassword)
                    {
						HttpContext.Session.SetString("Username", user.Username);
						return RedirectToAction("Index", "Home");
					}    					
				}
            }
            //ViewBag.ErrorMessage = errorMessage;
            return View("Auth", model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public IActionResult CheckEmailExists(string Email)
        {
            var emailExists = _context.Accounts.Any(x => x.Email == Email);
            return Json(!emailExists); // trả về true nếu email chưa tồn tại và ngược lại
        }
    }
}
