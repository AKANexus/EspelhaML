
using MlSuite.App.Services;
using MlSuite.Domain.Enums;

namespace MlSuite.App;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, JwtUtils jwtUtils, AccountBaseDataService accountDataService)
    {
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            (Guid uuid, Role role, Guid tenant)? accountUuid = await jwtUtils.ValidateJwt(token);
            if (accountUuid.HasValue)
            {
                context.Items["Account"] =
                    await accountDataService.GetByUuidAndRole(accountUuid.Value.uuid, accountUuid.Value.role);
                //context.Items["Tenant"] = accountUuid.Value.tenant;
            }
        }

        await _next(context);
    }
}