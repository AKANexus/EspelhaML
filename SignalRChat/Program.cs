using Npgsql;
using SignalRChat.Hubs;
using SignalRChat.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddSingleton<PostgresNotificationService>();
builder.Services.AddDistributedMemoryCache();

//NpgsqlConnectionStringBuilder csb = new()
//{
//    Database = "meliEspelho",
//    Port = 5351,
//    Username = "meliDBA",
//    Password = builder.Configuration.GetSection("SuperSecretSettings")["NpgPassword"],
//#if DEBUG
//    Host = "192.168.10.215"
//#else
//    Host = "tinformatica.dyndns.org"
//#endif
//};



builder.Services.AddSession(opts =>
{
    opts.Cookie.Name = ".MyTestingCookie.Session";
    opts.IdleTimeout = TimeSpan.FromSeconds(10);
    opts.Cookie.HttpOnly = true;
    opts.Cookie.IsEssential = true;
    opts.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

var app = builder.Build();


NotificationTask = app.Services.GetRequiredService<PostgresNotificationService>().MonitorForNotification();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/chatHub");

app.Run();
