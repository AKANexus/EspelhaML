using Microsoft.AspNetCore.Mvc;
using MlSuite.App.Models;
using System.Diagnostics;
using MlSuite.App.Attributes;

namespace MlSuite.App.Controllers
{
    //[Authorise]
    public class PagesController : Controller
    {
        private readonly ILogger<PagesController> _logger;

        public PagesController(ILogger<PagesController> logger)
        {
            _logger = logger;
        }

        
        public IActionResult Index()
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