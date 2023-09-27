using Microsoft.AspNetCore.Mvc;
using MlSuite.App.Attributes;

namespace MlSuite.App.Controllers
{
    public class AuthController : Controller
    {
        [Route("[controller]/login")]
        public async Task<IActionResult> LoginGet(string? status)
        {
            if (status == "NotLoggedIn")
            {
                TempData["message"] = "Por favor faça login para continuar";
            }
            return View("Login");
        }

        [Route("[controller]/register")]
        public async Task<IActionResult> RegisterGet()
        {
            return View("Register");
        }
    }
}
