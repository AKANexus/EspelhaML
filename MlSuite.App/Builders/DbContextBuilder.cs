using Microsoft.EntityFrameworkCore;
using MlSuite.EntityFramework.EntityFramework;
using Npgsql;

namespace MlSuite.App.Builders;

public static class DbContextBuilder
{

    public static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder, string dbPassword)
    {
        NpgsqlConnectionStringBuilder connStringBuilder = new();
        connStringBuilder.Host = "192.168.10.215:5351";
        connStringBuilder.Username = "meliDBA";
        connStringBuilder.Password = dbPassword;
        connStringBuilder.Database = "meliEspelho";
        //connStringBuilder.SslMode = SslMode.Prefer;

        Action<DbContextOptionsBuilder> configDbContext = c =>
        {
            c.UseNpgsql(connStringBuilder.ConnectionString);
            c.EnableDetailedErrors();
            c.EnableSensitiveDataLogging();
        };

        builder.Services.AddDbContext<TrilhaDbContext>(configDbContext);
        return builder;
    }
}