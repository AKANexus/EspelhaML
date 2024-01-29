using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MlSuite.Api.Attributes;
using MlSuite.Api.DTOs;
using MlSuite.Domain;
using MlSuite.EntityFramework.EntityFramework;

namespace MlSuite.Api.Controllers
{
    [Route("sellers")]
    public class SellersController : Controller
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public SellersController(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        [HttpGet(""), Autorizar]
        public async Task<IActionResult> GetAllSellers()
        {
            var provider = _scopeFactory.CreateScope().ServiceProvider;
            var context = provider.GetRequiredService<TrilhaDbContext>();

            List<MlUserAuthInfo> UserAuthInfos = context.MlUserAuthInfos.AsNoTracking().ToList();

            var retorno = new RetornoDto("Seller Ids encontrados",
                new {sellers = UserAuthInfos.Select(x => new { Conta = x.AccountNickname, SellerId = x.UserId })});
            return Ok(retorno);
        }
    }
}
