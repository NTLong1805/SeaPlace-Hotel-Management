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
using FirstProjectNET.ServiceFolder;

namespace FirstProjectNET.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly HotelDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;        
        public AuthenticationController(HotelDbContext context,UserManager<Account> userManager,SignInManager<Account> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            
        }
        /// <summary>
        /// Auth
        /// </summary>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>
        [HttpGet]
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
        public async Task<IActionResult> SignUp(AuthenticationViewModel model,string? ReturnUrl = null)
        {
            //         model.IsRegister = true;
            //         ModelState.Remove("SignInEmail");
            //         ModelState.Remove("SignInPassword");
            //         string ErrorMessage = "";
            //if (ModelState.IsValid)
            //{
            //	if (_context.Accounts.Any(u => u.Email == model.SignUpEmail))
            //	{
            //		ErrorMessage = "Email was existsed";
            //		return View("Auth", model);
            //	}
            //	string hashPassword = HashPassword(model.SignUpPassword);

            //	var newAccount = new Account
            //	{
            //		//AccountID = nextAccountID,
            //		Email = model.SignUpEmail,
            //		Password = hashPassword,
            //		Username = model.SignUpUserName,
            //		Type = Models.Common.AccountType.Admin,
            //		Active = true,
            //	};
            //	_context.Accounts.Add(newAccount);
            //	_context.SaveChanges();

            //	//ViewBag.ErrorMessage = "Successful Sign Up";
            //	HttpContext.Session.SetString("Username", model.SignUpUserName);
            //             HttpContext.Session.SetString("Type", newAccount.Type.ToString());
            //	return RedirectToAction("Index", "Home");
            //}
            //ViewBag.ErrorMessage = ErrorMessage;
            //return View("Auth", model);		
            model.IsRegister = true;
            ModelState.Remove("SignInEmail");
            ModelState.Remove("SignInPassword");
            string ErrorMessage = "";
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.SignUpEmail);
                if (existingUser != null)
                {
                    ErrorMessage = "Email already exists";
                    ViewBag.ErrorMessage = ErrorMessage;
                    return View("Auth", model);
                }
                

                string hashPassword = HashPassword(model.SignUpPassword);
                var newAccount = new Account
                {
                    Email = model.SignUpEmail,
                    Password = hashPassword, 
                    Username = model.SignUpUserName,
                    Type = Models.Common.AccountType.Customer,
                    Active = true
                };

                var result = await _userManager.CreateAsync(newAccount);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(newAccount, isPersistent: false);

                    HttpContext.Session.SetString("Username", model.SignUpUserName);
                    HttpContext.Session.SetString("Type", newAccount.Type.ToString());

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            ViewBag.ErrorMessage = ErrorMessage;
            return View("Auth", model);

        }

        /// <summary>
        /// HashPassword
        /// </summary>
        /// <param name="signUpPassword"></param>
        /// <returns></returns>
		private string HashPassword(string signUpPassword)
		{
			using (var sha256 = SHA256.Create())
			{
				var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(signUpPassword));
				return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
			}
		}
        /// <summary>
        /// SignIn
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ReturnUrl"></param>
        /// <returns></returns>

		[HttpPost]
        public async Task<IActionResult> SignIn(AuthenticationViewModel model,string? ReturnUrl = "/")
        {
			//        model.IsRegister = false;
			//        ModelState.Remove("SignUpEmail");
			//        ModelState.Remove("SignUpUsername");
			//        ModelState.Remove("SignUpPassword");
			//        ModelState.Remove("SignUpConfirmPassword");
			//        string errorMessage = "";

			//        if (ModelState.IsValid)
			//        {
			//            if (HttpContext.Session.GetString("Username") == null)
			//            {
			//                var user = _context.Accounts.FirstOrDefault(u => u.Email == model.SignInEmail);                    
			//                if (user == null)
			//                {
			//		ViewBag.ErrorMessage = "Account does not exists";
			//                    ViewData["ReturnUrl"] = ReturnUrl;
			//                    return View("Auth",model);
			//                }
			//                string hashedPassword = HashPassword(model.SignInPassword);
			//                if(user.Password != hashedPassword)
			//                {
			//                    ViewBag.ErrorMessage = "Wrong Password";
			//		ViewData["ReturnUrl"] = ReturnUrl;
			//		return View("Auth", model);
			//                }

			//	HttpContext.Session.SetString("Username", user.Username);                    
			//                HttpContext.Session.SetString("Type", user.Type.ToString());

			//	if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
			//	{						
			//		return Redirect(model.ReturnUrl);
			//	}
			//	return RedirectToAction("Index", "Home");		

			//}
			//        }
			//        //ViewBag.ErrorMessage = errorMessage;
			//        return View("Auth", model);
			model.IsRegister = false;
			ModelState.Remove("SignUpEmail");
			ModelState.Remove("SignUpUsername");
			ModelState.Remove("SignUpPassword");
			ModelState.Remove("SignUpConfirmPassword");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (HttpContext.Session.GetString("Username") == null)
            {
                var user = await _userManager.FindByEmailAsync(model.SignInEmail);
                if (user == null)
                {
                    ViewBag.ErrorMessage = "Account does not exist";
                    return View("Auth", model);
                }


                string hashPashword = HashPassword(model.SignInPassword);
                //var result = await _userManager.CheckPasswordAsync(user, model.SignInPassword);
                if (hashPashword != user.Password)
                {
                    ViewBag.ErrorMessage = "Wrong email or password";
                    return View("Auth", model);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);

                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Type", user.Type.ToString());

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            return View("Auth", model);
        }
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            //HttpContext.Session.Clear();
            //HttpContext.Session.Remove("Username");
            //HttpContext.SignOutAsync();

            //return RedirectToAction("Index","Home");
            await _signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index","Home");
        }

        /// <summary>
        /// CheckEmailExists  -  Client side validate
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CheckEmailExists(string Email)
        {
            var emailExists = _context.Accounts.Any(x => x.Email == Email);
            return Json(!emailExists); 
        }

        /// <summary>
        /// ExternalLogin
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ExternalLogin(string provider,string returnUrl ="/")
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Authentication", new { returnUrl});

            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            return Challenge(properties, provider);
        }

        /// <summary>
        /// ExternalLoginCallback
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="remoteError"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl, string? remoteError)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                return Content($"<script>alert('Error from external provider: {remoteError}'); window.close();</script>", "text/html");
            }
            var authenticateResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!authenticateResult.Succeeded)
            {
                return Content($"<script>alert('Error loading external login information.'); window.close();</script>", "text/html");
            }
            var email = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = authenticateResult.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var loginProvider = authenticateResult.Principal.FindFirst(ClaimTypes.AuthenticationMethod)?.Value
                                                                ?? authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Issuer;
            var providerKey = authenticateResult.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (email == null || loginProvider == null || providerKey == null)
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
            //var existingLogin = await _context.AccountLogins.FirstOrDefaultAsync(x => x.AccountID == account.AccountID && x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
            //if (existingLogin == null)
            //{
            //    var externalLogin = new AccountLogin
            //    {
            //        AccountID = account.AccountID,
            //        LoginProvider = loginProvider,
            //        ProviderKey = providerKey
            //    };
            //}
            HttpContext.Session.SetString("Username", account.Username);
            HttpContext.Session.SetString("Type", account.Type.ToString());
            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,new ClaimsPrincipal(authenticateResult.Principal));
            return Content($"<script>window.opener.location.href = '{returnUrl}'; window.close();</script>", "text/html");

            //var info = await _signInManager.GetExternalLoginInfoAsync();
            //if (info == null)
            //{
            //    return Content($"<script>alert('Error loading external login information.'); window.close();</script>", "text/html");
            //}
            //var user = await _userManager.FindByLoginAsync(info.LoginProvider,info.ProviderKey);
            //if (user != null)
            //{
            //    await _signInManager.SignInAsync(user, isPersistent: false);
            //    return Content($"<script>window.opener.location.href = '{returnUrl}'; window.close();</script>", "text/html");
            //}

            //var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            //if (email != null)
            //{
            //    user = await _userManager.FindByEmailAsync(email);
            //    if (user == null)
            //    {
            //        user = new Account
            //        {
            //            Username = info.Principal.FindFirstValue(ClaimTypes.GivenName) + " " + info.Principal.FindFirstValue(ClaimTypes.Surname),
            //            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
            //            Password = HashPassword("12345678"),
            //            Type = AccountType.Customer,
            //            Active = true
            //        };
            //        await _userManager.CreateAsync(user);
            //    }
            //    await _userManager.AddLoginAsync(user, info);
            //    await _signInManager.SignInAsync(user, isPersistent: false);
            //    return Content($"<script>window.opener.location.href = '{returnUrl}'; window.close();</script>", "text/html");
            //}
            //return Content($"<script>alert('Email claim not received. Please contact support.'); window.close();</script>", "text/html");
        }
        
    }
}
