using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MlSuite.Api.Attributes;
using MlSuite.Api.DTOs;
using MlSuite.Domain;
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

			var pedidoTentativo = await context.Pedidos.Include(pedido => pedido.Itens)
				.ThenInclude(pedidoItem => pedidoItem.Item).FirstOrDefaultAsync(x => x.Id == id);
			if (pedidoTentativo == null)
			{
				return NotFound(new RetornoDto("Sucesso", erro: "Nenhum registro encontrado"));
			}

			PedidoDto dto = new PedidoDto();
			dto.NúmPedido = pedidoTentativo.Id;
			dto.PedidoItems = pedidoTentativo.Itens.Select(x =>
				new PedidoItemDto
				{
					Sku = x.Sku,
					Descrição = x.Item.Título,
					Quantidade = x.QuantidadeVendida,
					UrlImagem = x.Item.PrimeiraFoto

				}).ToList();

			return Ok(new RetornoDto("Sucesso", dto));
		}

		[HttpGet, Autorizar]
		public async Task<IActionResult> GetSeparaçãoEmAberto()
		{
			object? userUuid = HttpContext.Items["userInfo"];
			if (userUuid is not UserInfo userInfo)
			{
				return BadRequest("Uuid não foi determinada.");
			}

			var provider = _scopeFactory.CreateScope().ServiceProvider;
			var context = provider.GetRequiredService<TrilhaDbContext>();

			UserInfo? requestingUser = await context.Usuários.FirstOrDefaultAsync(x =>
				x.Uuid == userInfo.Uuid);

			if (requestingUser == null)
			{
				return BadRequest("Usuário inexistente");
			}



			var pedidoTentativo = await context.Pedidos
				.Include(x => x.Separação)
				.ThenInclude(separação => separação!.Usuário)
				.Include(x => x.Itens)
				.ThenInclude(y => y.Separação).Include(pedido => pedido.Itens)
				.ThenInclude(pedidoItem => pedidoItem.Item)
				//TODO: Order by qual eixo? - Regra de Negócio
				.FirstOrDefaultAsync(x => x.Separação != null && x.Separação.Usuário.Uuid == requestingUser.Uuid);
			
			if (pedidoTentativo == null)
			{
				return NotFound(new RetornoDto("Sucesso", "Nenhum dado encontrado"));
			}
			else
			{
				PedidoDto dto = new PedidoDto();
				dto.NúmPedido = pedidoTentativo.Id;
				dto.SeparadoPor = pedidoTentativo.Separação!.Usuário.DisplayName;
				dto.PedidoItems = pedidoTentativo.Itens.Select(x =>
					new PedidoItemDto
					{
						Sku = x.Sku,
						Descrição = x.Item!.Título,
						Quantidade = x.QuantidadeVendida,
						UrlImagem = x.Item.PrimeiraFoto,
						Separados = x.Separação?.Separados ?? 0

					}).ToList();
				return Ok(new RetornoDto("Sucesso", dto));
			}
		}

		[HttpGet("{sku}"), Autorizar]
		public async Task<IActionResult> GetNextPedido(string sku)
		{
			var provider = _scopeFactory.CreateScope().ServiceProvider;
			var context = provider.GetRequiredService<TrilhaDbContext>();

			object? userUuid = HttpContext.Items["userInfo"];
			if (userUuid is not UserInfo userInfo)
			{
				return BadRequest("Uuid não foi determinada.");
			}

			UserInfo? requestingUser = await context.Usuários.FirstOrDefaultAsync(x =>
				x.Uuid == userInfo.Uuid);

			if (requestingUser == null)
			{
				return BadRequest("Usuário inexistente");
			}

			var pedidoTentativo = await context.Pedidos.Include(pedido => pedido.Itens)
				.ThenInclude(pedidoItem => pedidoItem.Item)
				.Where(x=>x.Itens.Any(y=>y.Sku == sku) && x.Separação == null)
				//TODO: Order by qual eixo? - Regra de Negócio
				.FirstOrDefaultAsync();

			if (pedidoTentativo == null)
			{
				return NotFound(new RetornoDto("Sucesso", erro: "Nenhum registro encontrado."));
			}
			else
			{
				PedidoDto dto = new PedidoDto();
				dto.NúmPedido = pedidoTentativo.Id;
				dto.PedidoItems = pedidoTentativo.Itens.Select(x =>
					new PedidoItemDto
					{
						Sku = x.Sku,
						Descrição = x.Item.Título,
						Quantidade = x.QuantidadeVendida,
						UrlImagem = x.Item.PrimeiraFoto

					}).ToList();

				pedidoTentativo.Separação = new Separação()
				{
					Início = DateTime.Now,
					Usuário = requestingUser,
					Pedido = pedidoTentativo,
				};

				context.Update(pedidoTentativo);
				try
				{
					await context.SaveChangesAsync();
				}
				catch (Exception e)
				{
					return StatusCode(500, e);
				}
				return Ok(new RetornoDto("Sucesso", dto));
			}
		}
	}
}
