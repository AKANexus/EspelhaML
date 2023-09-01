using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace SignalRChat.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
