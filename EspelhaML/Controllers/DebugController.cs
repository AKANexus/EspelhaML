using EspelhaML.Domain;
using EspelhaML.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace EspelhaML.Controllers
{
    [Route("debug"), ApiController]
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
    }
}
