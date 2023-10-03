using Microsoft.AspNetCore.Mvc;

namespace MlSuite.App.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
