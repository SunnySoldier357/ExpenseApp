using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseApp.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult SignIn()
        {
            string redirectUrl = Url.Action("Index", "Form");

            return Challenge(
                new AuthenticationProperties { RedirectUri = redirectUrl },
                OpenIdConnectDefaults.AuthenticationScheme);
        }
        
        public IActionResult SignOut()
        {
            string callbackUrl = Url.Action("SignedOut", "Auth",
                values: null, protocol: Request.Scheme);

            return SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }
        
        public IActionResult SignedOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to home page if user is authenticated
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}