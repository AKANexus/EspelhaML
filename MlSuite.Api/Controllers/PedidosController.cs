using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MlSuite.Api.DTOs;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.Api.Controllers
{
	[Route("pedidos")]
	public class PedidosController : Controller
	{
		private readonly IServiceScopeFactory _scopeFactory;

		public PedidosController(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetPedidoById(ulong id)
		{
			var provider = _scopeFactory.CreateScope().ServiceProvider;
			var context = provider.GetRequiredService<TrilhaDbContext>();

			var pedidoTentativo = await context.Pedidos.FirstOrDefaultAsync(x => x.Id == id);
			if (pedidoTentativo == null)
			{
				return NotFound(new RetornoDto("Sucesso", "Nenhum registro encontrado"));
			}

			PedidoDto dto = new PedidoDto();
			dto.NúmPedido = pedidoTentativo.Id;
			//dto.PedidoItems = 

			return Ok(new RetornoDto("Sucesso", new PedidoDto()));
		}
	}
}
