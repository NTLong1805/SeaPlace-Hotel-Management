using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FirstProjectNET.Models.ViewModel;
using FirstProjectNET.Data;
using FirstProjectNET.Models;
using FirstProjectNET.Models.Common;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.WebSockets;

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
        [AllowAnonymous]
        public IActionResult Auth(string? ReturnUrl = null)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
			
			// kiểm tra xem đã có đăng nhập chưa
			if (HttpContext.Session.GetString("Username") == null)
            {
                return View(new AuthenticationViewModel());
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
					Type = Models.Common.AccountType.Admin,
					Active = true,
				};
				_context.Accounts.Add(newAccount);
				_context.SaveChanges();

				//ViewBag.ErrorMessage = "Successful Sign Up";
				HttpContext.Session.SetString("Username", model.SignUpUserName);
                HttpContext.Session.SetString("Type", newAccount.Type.ToString());
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
        public async Task<IActionResult> SignIn(AuthenticationViewModel model,string? ReturnUrl)
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
                        ViewData["ReturnUrl"] = ReturnUrl;
                        return View("Auth",model);
                    }
                    string hashedPassword = HashPassword(model.SignInPassword);
                    if(user.Password != hashedPassword)
                    {
                        ViewBag.ErrorMessage = "Wrong Password";
						ViewData["ReturnUrl"] = ReturnUrl;
						return View("Auth", model);
                    }
					
					HttpContext.Session.SetString("Username", user.Username);                    
                    HttpContext.Session.SetString("Type", user.Type.ToString());
                    
					if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
					{						
						return Redirect(model.ReturnUrl);
					}
					return RedirectToAction("Index", "Home");		
					
				}
            }
            //ViewBag.ErrorMessage = errorMessage;
            return View("Auth", model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            HttpContext.SignOutAsync();

            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public IActionResult CheckEmailExists(string Email)
        {
            var emailExists = _context.Accounts.Any(x => x.Email == Email);
            return Json(!emailExists); // trả về true nếu email chưa tồn tại và ngược lại
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ExternalLogin(string provider,string returnUrl ="/")
        {
			//var redirectUrl = Url.Action("ExternalLoginCallback","Authentication",new {returnUrl});
			var redirectUrl = Url.Action("ExternalLoginCallback", "Authentication", new { ReturnUrl = returnUrl });

			var properties = new AuthenticationProperties { RedirectUri = redirectUrl};

            return Challenge(properties, provider);
        }
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl, string? remoteError)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                return Content($"<script>alert('Error from external provider: {remoteError}'); window.close();</script>", "text/html");
            }
            var authenticateResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return Content($"<script>alert('Error loading external login information.'); window.close();</script>", "text/html");
            }
            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;
            if(email == null)
            {
				return Content($"<script>alert('Email not received from Google.'); window.close();</script>", "text/html");
			}    
            var account = _context.Accounts.FirstOrDefault(x => x.Email == email);            
            if (account == null)
            {
                account = new Account
                {
                    Email = email,
                    Username = authenticateResult.Principal.FindFirst(ClaimTypes.GivenName).Value + " " + authenticateResult.Principal.FindFirst(ClaimTypes.Surname).Value,
                    Password = HashPassword("12345678"),
                    Type = AccountType.Customer,
                    Active = true
                };
                _context.Accounts.Add(account);
                _context.SaveChanges();
            }
            HttpContext.Session.SetString("Username", account.Username);
            HttpContext.Session.SetString("Type",account.Type.ToString());

			return Content($"<script>window.opener.location.href = '{returnUrl}'; window.close();</script>", "text/html");
		}
        
    }
}
