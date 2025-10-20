var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Agregá todos los servicios antes de Build()
builder.Services.AddControllersWithViews();
builder.Services.AddSession();  // 👈 importante, antes de builder.Build()

// 2️⃣ Luego construís la app
var app = builder.Build();

// 3️⃣ Y recién después configurás el pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession(); // 👈 esto va después de UseAuthorization()

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
