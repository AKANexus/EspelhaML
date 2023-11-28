using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MlSuite.Api;
using MlSuite.Api.Middlewares;
using MlSuite.Api.Services;
using MlSuite.Domain;
using MlSuite.EntityFramework.EntityFramework;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

NpgsqlConnectionStringBuilder csb = new()
{
	Database = "meliEspelho",
	Port = 5432,
	Username = "meliDBA",
	Password = builder.Configuration.GetSection("SuperSecretSettings")["NpgPassword"],
//#if DEBUG
	Host = "ec2-15-228-160-231.sa-east-1.compute.amazonaws.com"
//#else
//    Host = "localhost"
//#endif

};
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
Action<DbContextOptionsBuilder> configureDbContext = c =>
{
	c.UseNpgsql(csb.ConnectionString, b =>
	{
		b.MigrationsAssembly("MlSuite.MlSynch");
	});
	c.EnableSensitiveDataLogging(true);
	c.EnableDetailedErrors(true);
};

builder.Services.AddDbContext<TrilhaDbContext>(configureDbContext);
builder.Services.AddScoped<RefreshTokenGenerator>();
builder.Services.AddScoped<JwtUtils>();
builder.Services.AddScoped<EmailService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.UseMiddleware<JwtMware>();

app.MapControllers();

app.Run();
