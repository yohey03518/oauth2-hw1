using User.Web.Controllers;
using User.Web.Middlewares;
using User.Web.Proxies;
using User.Web.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped<OnlineUserManager>();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<LineNotifyProxy>();
builder.Services.AddTransient<LineLoginProxy>();
// builder.Services.AddTransient<IApplicationUserRepository, InMemoryUserRepository>();
builder.Services.AddTransient<IApplicationUserRepository, InFileApplicationUserRepository>();
// builder.Services.AddTransient<IPublishRecordRepository, InMemoryPublishRecordRepository>();
builder.Services.AddTransient<IPublishRecordRepository, InFilePublishRecordRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<OnlineUserHandler>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();