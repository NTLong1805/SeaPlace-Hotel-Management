
using FirstProjectNET.Data;
using FirstProjectNET.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Đọc ConnectionString từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("SQLServerConnection") ?? throw new InvalidOperationException("Connection string 'SQLServerIdentityConnection' not found.");

// 🔹 Đăng ký DbContext trước khi gọi `builder.Build()`
builder.Services.AddDbContext<HotelDbContext>(options => options.UseSqlServer(connectionString));
// 🔹 Thêm Controllers với Views
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // thời gian session là 30 phút 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Authentication/Auth";
})
.AddGoogle(options =>
{
   options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
   options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
})
.AddFacebook(options =>
{
    options.ClientId = builder.Configuration["Authentication:Facebook:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
});

builder.Services.AddScoped<EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services);
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.UseAuthentication();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
