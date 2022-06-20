using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Funding.Provider.Web.Infrastructure.Authorisation;
using SFA.DAS.Funding.Provider.Web.Models;
using System.Diagnostics;

namespace SFA.DAS.Funding.Provider.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        [AllowAnonymous()]
        public async Task<IActionResult> AnonymousHome()
        {
            return RedirectToAction("login");
        }

        [Route("/login")]
        public async Task<IActionResult> Login()
        {
            if (User.HasClaim(c => c.Type.Equals(ProviderClaims.UserId)))
            {
                // return RedirectToAction("Home", new { accountId = User.Claims.First(c => c.Type.Equals(EmployerClaimTypes.Account)).Value });
            }
            return Forbid();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}