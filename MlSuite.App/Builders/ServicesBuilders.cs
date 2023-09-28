using MlSuite.App.Services;
using MlSuite.Domain;

namespace MlSuite.App.Builders;

public static class ServicesBuilders
{
    public static WebApplicationBuilder AddDataServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<AccountBaseDataService>();
        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<AccountsService>();
        builder.Services.AddScoped<EmailService>();
        builder.Services.AddScoped<JwtUtils>();
        builder.Services.AddScoped<VerificationTokenService>();
        builder.Services.AddScoped<PromolimitDataService>();
        return builder;
    }
}