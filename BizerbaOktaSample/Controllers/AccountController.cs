using BizerbaOktaSample.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Okta.AspNetCore;

namespace BizerbaOktaSample.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(IAuthorizationGateway authGateway)
        {
            this._authGateway = authGateway;
        }
        
        public IActionResult Login()
        {
            if (!this.HttpContext.User.Identity.IsAuthenticated)
            {
                AuthenticationProperties properties = new AuthenticationProperties
                {
                    RedirectUri = this.Url.Action("LoginCallback"),
                    Items =
                    {
                        {
                            "scheme", OktaDefaults.MvcAuthenticationScheme
                        }
                    }
                };

                return Challenge(properties, OktaDefaults.MvcAuthenticationScheme);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult LoginCallback()
        {
            // Do some setup here
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return new SignOutResult(new[]
            {
                OktaDefaults.MvcAuthenticationScheme,
                CookieAuthenticationDefaults.AuthenticationScheme,
            });
        }

        private readonly IAuthorizationGateway _authGateway;
    }
}