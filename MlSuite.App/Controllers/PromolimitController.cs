using System.Net.NetworkInformation;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MlSuite.App.Models;
using MlSuite.App.Services;
using MlSuite.Domain;
using MlSuite.Domain.Entities;
using MlSuite.EntityFramework.EntityFramework;
using MlSuite.MlApiServiceLib;

namespace MlSuite.App.Controllers
{
    public class PromolimitController : Controller
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PromolimitController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDataPaged([FromQuery] FilteredQuery? filteredQueryModel)
        {
            var scope = _scopeFactory.CreateScope();
            PromolimitDataService promolimitDataService =
                scope.ServiceProvider.GetRequiredService<PromolimitDataService>();
            if (filteredQueryModel == null)
            {
                return BadRequest();
            }

            if (filteredQueryModel.Filters.All(x => string.IsNullOrWhiteSpace(x.Field) && string.IsNullOrWhiteSpace(x.Query)))
            {
                filteredQueryModel.Filters = new Filter[] { };
            }
            var produtos = await promolimitDataService.GetAllProdutosPaged(filteredQueryModel);

            List<SaveMlbEntryJson> entriesList = new();

            foreach (PromolimitEntry entry in produtos)
            {
                entriesList.Add(new()
                {
                    Id = entry.Uuid,
                    Seller = entry.Item.Seller.AccountNickname,
                    Descricao = entry.Item.Título,
                    QuantidadeAVenda = entry.QuantidadeAVenda.ToString(),
                    Estoque = entry.Estoque,
                    MLB = entry.Item.Id
                });
            }

            var count = await promolimitDataService.CountProdutos();

            return Json(new
            {
                returnedData = entriesList,
                currentPage = Math.Ceiling(filteredQueryModel.offset ?? 0 / filteredQueryModel.limit ?? 1M),
                maxPages = Math.Ceiling(count / (filteredQueryModel.limit ?? 1M)),
                recordsPerPage = (filteredQueryModel.limit ?? 1M),
                recordsTotal = count
            });
        }

        public class SaveMlbEntryJson
        {
            public string MLB { get; set; }
            public Guid Id { get; set; }
            public string QuantidadeAVenda { get; set; }
            public int? Estoque { get; set; }
            public bool Ativo { get; set; }
            public string? Descricao { get; set; }
            public string? Seller { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> SaveMlbEntry([FromBody] SaveMlbEntryJson? produto)
        {
            var scope = _scopeFactory.CreateScope();
            var mlApiService = scope.ServiceProvider.GetRequiredService<MlApiService>();
            var context = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();

            if (produto is null) return BadRequest(new { success = false, error = "Produto was null" });
            if (produto.MLB[..3] != "MLB")
                produto.MLB = "MLB" + produto.MLB;
            var consultaMl = await mlApiService.GetItemById(produto.MLB);
            if (consultaMl.Item2 is null)
            {
                TempData["Error"] = "Erro. Tente novamente.";
                return Json(new { success = false, error = "Erro. Tente novamente." });
            }

            if (await context.MlUserAuthInfos.AllAsync(x => x.UserId != consultaMl.Item2.SellerId))
            {
                TempData["Error"] = "O MLB informado não é de nenhuma conta cadastrada no sistema.";
                return Json(new { success = false, error = "O MLB informado não é de nenhuma conta cadastrada no sistema." });
            }


            if (true)
            {
                if (consultaMl.Item2?.Variations is not null && consultaMl.Item2.Variations.Count > 0)
                {
                    foreach (var variation in consultaMl.Item2.Variations)
                    {
                        //if (produtos.Any(x => x.MLB == consultaMl.Item2.Id && x.Variacao == variation.Id))
                        //{
                        var mlUserInfo = await context.MlUserAuthInfos.AsNoTracking()
                            .FirstOrDefaultAsync(x => x.UserId == consultaMl.Item2.SellerId);
                        if (mlUserInfo == null)
                        {
                            return Json(new { success = false, error = $"mlUserInfo não encontrado: {consultaMl.Item2.SellerId}" });
                        }
                        var atualiza = await mlApiService.AtualizaEstoqueDisponivel(mlUserInfo.AccessToken, consultaMl.Item2.Id, int.Parse(produto.QuantidadeAVenda), variation.Id);
                        if (!atualiza.Item1)
                        {
                            TempData["Error"] = atualiza.Item2;
                            return Json(new { success = false, error = atualiza.Item2 });
                        }
                        //}
                        StringBuilder sb = new();
                        foreach (var attributeCombination in variation.AttributeCombinations)
                        {
                            sb.Append($"{attributeCombination.Name}:{attributeCombination.ValueName}");
                        }

                        var itemTentativo =
                            await context.Itens
                                .Include(x => x.Variações)
                                .FirstOrDefaultAsync(x => x.Id == produto.MLB);
                        if (itemTentativo is null)
                            return Json(new { success = false });
                        PromolimitEntry aGravar = new()
                        {
                            Item = itemTentativo,
                            Ativo = true,
                            QuantidadeAVenda = int.Parse(produto.QuantidadeAVenda),
                            Estoque = produto.Estoque ?? 0,
                            Variação = itemTentativo.Variações.FirstOrDefault(x => x.Id == variation.Id)
                        };
                        context.PromolimitEntries.Update(aGravar);
                        await context.SaveChangesAsync();
                    }
                }
                else
                {
                    var mlUserInfo = await context.MlUserAuthInfos.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.UserId == consultaMl.Item2.SellerId);
                    if (mlUserInfo == null)
                    {
                        return Json(new { success = false, error = $"mlUserInfo não encontrado: {consultaMl.Item2.SellerId}" });
                    }
                    var atualiza = await mlApiService.AtualizaEstoqueDisponivel(mlUserInfo.AccessToken, consultaMl.Item2.Id, int.Parse(produto.QuantidadeAVenda));
                    if (!atualiza.Item1)
                    {
                        return Json(new { success = false, error = atualiza.Item2 });
                    }
                    var itemTentativo =
                        await context.Itens
                            .FirstOrDefaultAsync(x => x.Id == produto.MLB);
                    if (itemTentativo is null)
                        return Json(new { success = false });
                    PromolimitEntry aGravar = new()
                    {
                        Item = itemTentativo,
                        Ativo = true,
                        QuantidadeAVenda = int.Parse(produto.QuantidadeAVenda),
                        Estoque = produto.Estoque ?? 0,
                    };
                    context.PromolimitEntries.Update(aGravar);
                    await context.SaveChangesAsync();
                }

                return Json(new { success = true });
            }
        }

        [HttpGet]
        public async Task<IActionResult> RemoveMlbEntry([FromQuery] Guid id)
        {
            var scope = _scopeFactory.CreateScope();
            //var mlApiService = scope.ServiceProvider.GetRequiredService<MlApiService>();
            var context = scope.ServiceProvider.GetRequiredService<TrilhaDbContext>();
            var tentativo = await context.PromolimitEntries.FirstOrDefaultAsync(x => x.Uuid == id);
            if (tentativo is not null)
            {
                context.PromolimitEntries.Remove(tentativo);
                await context.SaveChangesAsync();
            }
            return Json(new { success = true });
        }

        [HttpGet("verificaAnuncios")]
        public async Task<IActionResult> VerificaAnunciosGet()
        {
            if (Verificando)
            {
                TempData["Error"] = "O sistema já está rodando a verificação dos itens. Tente novamente mais tarde";
                return Ok(TempData["Error"]);
            }
            else
            {
                Verificando = true;
                return View("VerificaAnuncios");
            }
        }
    }
}
