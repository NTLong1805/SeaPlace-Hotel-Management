using FirstProjectNET.Data;
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
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
