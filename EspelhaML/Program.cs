using Microsoft.EntityFrameworkCore;
using MlSuite.EntityFramework.EntityFramework;
using MlSuite.MlApiServiceLib;
using MlSuite.MlSynch.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddScoped(_ => new MlApiService(
    builder.Configuration.GetSection("SuperSecretSettings")["ClientId"] ?? throw new NullReferenceException("ClientID não pode ser nulo"),
    builder.Configuration.GetSection("SuperSecretSettings")["ClientSecret"] ?? throw new NullReferenceException("Client secret não pode ser nulo"),
    builder.Configuration.GetSection("SuperSecretSettings")["RedirectUrl"] ?? throw new NullReferenceException("Redirect URL não pode ser nulo")
));
builder.Services.AddScoped<ProcessQuestionService>();
builder.Services.AddScoped<ProcessItemService>();
builder.Services.AddScoped<ProcessOrderService>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
