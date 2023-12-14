using Microsoft.AspNetCore.Mvc;
using MlSuite.Api.DTOs;

namespace MlSuite.Api.Controllers
{
	[Route("")]
	public class DebugController : Controller
	{
		[HttpGet("")]
		public IActionResult Index()
		{
			return Ok("I'm alive.");
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
        public IActionResult Login([FromBody] LoginDto dto)
        {

            if (dto == null)
            {
                return BadRequest(new RetornoDto("Informações inválidas"));
            }

            if (dto.Username == null || dto.Password == null)
            {
                return BadRequest(new RetornoDto("Informações incompletas"));
            }
			Response.Cookies.Append("cookie_de_login", Guid.NewGuid().ToString(), new CookieOptions()
            {
				HttpOnly = false,
				Expires = DateTime.UtcNow.AddHours(5),
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
