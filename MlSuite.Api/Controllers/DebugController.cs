using Microsoft.AspNetCore.Mvc;
using MlSuite.Api.DTOs;

namespace MlSuite.Api.Controllers
{
	[Route("debug")]
	public class DebugController : Controller
	{
		[HttpGet("index")]
		public IActionResult Index()
		{
			var cookie1 = Request.Cookies["cookie1"];
			var cookie2 = Request.Cookies["cookie2"];
			return Ok();
		}

		[HttpGet("givemeacookie")]
		public IActionResult GiveMeACookie()
		{
			Response.Cookies.Append("cookie1", "Stale Cookie", new CookieOptions()
			{
				Expires = DateTimeOffset.UtcNow.AddSeconds(10),
				HttpOnly = true
			});
			Response.Cookies.Append("cookie2", "Chocolate Chips Cookie", new CookieOptions()
			{
				Expires = DateTimeOffset.UtcNow.AddHours(30),
				HttpOnly = true
			});
			return Ok();
		}

        [HttpPost("fakeLogin")]
        public IActionResult Login(LoginDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new RetornoDto("Falha ao fazer login", "Informações inválidas"));
            }

            if (dto.Username == null || dto.Password == null)
            {
                return BadRequest(new RetornoDto("Falha ao fazer login", "Informações incompletas"));
            }
			HttpContext.Response.Cookies.Append("cookie_de_login", Guid.NewGuid().ToString(), new CookieOptions()
            {
				HttpOnly = true,
				Expires = DateTime.UtcNow.AddMinutes(5),
            });
            return Ok();
        }
	}

    public class LoginDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; } 
    }
}
