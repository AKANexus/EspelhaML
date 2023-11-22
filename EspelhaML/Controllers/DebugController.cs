using Microsoft.AspNetCore.Mvc;
using MlSuite.Domain;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.MlSynch.Controllers
{
    [Route(""), ApiController]
    public class DebugController : ControllerBase
    {
        private readonly IServiceProvider _provider;

        public DebugController(IServiceProvider provider)
        {
            _provider = provider;
        }
        [HttpGet("WriteNewLog")]
        public async Task<IActionResult> WriteNewLog(string message)
        {
            using IServiceScope scope = _provider.CreateScope();
            TrilhaDbContext context = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();


            context.Logs.Add(new EspelhoLog(nameof(WriteNewLog), message) {CreatedAt = DateTime.Now, UpdatedAt = DateTime.UtcNow});
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("")]
        public async Task<IActionResult> WriteNewLog()
        {
			return Ok("Yes, it is all OK");
        }
	}
}
