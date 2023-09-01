using Microsoft.AspNetCore.Mvc;
using SignalRChat.Models;
using System.Diagnostics;
using System.Text.Json;

namespace SignalRChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TestCookie()
        {
            int? cookieInt = HttpContext.Session.GetInt32("sessionId");
            cookieInt??=1;
            cookieInt++;
            HttpContext.Session.SetInt32("sessionId", (int)cookieInt!);
            return Ok(new {Message = $"The session ID is now {cookieInt}"});
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