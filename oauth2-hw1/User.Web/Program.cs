using Microsoft.AspNetCore.CookiePolicy;
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
builder.Services.AddTransient<IApplicationUserRepository, InMemoryUserRepository>();
builder.Services.AddTransient<IPublishRecordRepository, InMemoryPublishRecordRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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