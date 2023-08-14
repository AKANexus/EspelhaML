using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EspelhaML.Controllers
{
    [Route("v1/[controller]"), ApiController]
    public class CallbackController : ControllerBase
    {
        public IActionResult MlRedirect([FromQuery] string code)
        {

        }
    }
}
