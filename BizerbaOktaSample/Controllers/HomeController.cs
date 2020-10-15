using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BizerbaOktaSample.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return this.HttpContext.User.Identity.IsAuthenticated
                ? (IActionResult) RedirectToAction("Dashboard", "Home")
                : View();
        }
        
        [Authorize]
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}