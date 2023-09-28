using MlSuite.App.DTO.Configuration;
using System.Text.Json.Serialization;
using System.Text.Json;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using MlSuite.App.Builders;
using FluentValidation;
using MlSuite.App.Helpers;
using MlSuite.App;


var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("SuperSecretSettings"));

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        x.JsonSerializerOptions.WriteIndented = false;
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        x.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddFluentValidationAutoValidation();

//builder.Services.Configure<ApiBehaviorOptions>(opts =>
//{
//    opts.SuppressModelStateInvalidFilter = true;
//});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opts =>
{
    opts.IdleTimeout = TimeSpan.FromMinutes(20);
    opts.Cookie.HttpOnly = true;
    opts.Cookie.IsEssential = true;
});

builder.AddDbContext(builder.Configuration.GetSection("SuperSeecretSettings")["PostgresSqlPassword"]!);
builder.AddDataServices();
builder.AddServices();

ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;
ValidatorOptions.Global.DisplayNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Index/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=pages}/{action=index}/{id?}");

app.Run();
