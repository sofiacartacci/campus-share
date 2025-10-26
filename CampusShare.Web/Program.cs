using Microsoft.EntityFrameworkCore;
using CampusShare.Web.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CampusShareDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CampusShareDBConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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

app.Run();
