using Microsoft.EntityFrameworkCore;
using MlSuite.EntityFramework.EntityFramework;
using MlSuite.MlSynch.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<MlApiService>();
builder.Services.AddScoped<ProcessQuestionService>();
builder.Services.AddScoped<ProcessItemService>();
builder.Services.AddScoped<ProcessOrderService>();

NpgsqlConnectionStringBuilder csb = new()
{
    Database = "meliEspelho",
    Port = 5351,
    Username = "meliDBA",
    Password = builder.Configuration.GetSection("SuperSecretSettings")["NpgPassword"],
    #if DEBUG
    Host = "192.168.10.215"
    #else
    Host = "tinformatica.dyndns.org"
    #endif

};
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
Action<DbContextOptionsBuilder> configureDbContext = c =>
{
    c.UseNpgsql(csb.ConnectionString);
    c.EnableSensitiveDataLogging(true);
    c.EnableDetailedErrors(true);
};

builder.Services.AddDbContext<TrilhaDbContext>(configureDbContext);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
