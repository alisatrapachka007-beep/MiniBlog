using Microsoft.EntityFrameworkCore;
using MiniBlog.Data;
using MiniBlog.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры с представлениями
builder.Services.AddControllersWithViews();

// Регистрируем DbContext с SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=minblog.db"));

// Регистрируем сервисы (DI)
builder.Services.AddScoped<IPostService, PostService>();

var app = builder.Build();

// Настройка конвейера
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}");

app.Run();