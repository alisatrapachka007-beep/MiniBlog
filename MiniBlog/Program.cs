using Microsoft.EntityFrameworkCore;
using MiniBlog.Data;
using MiniBlog.Services;

var builder = WebApplication.CreateBuilder(args);

var dbPath = Environment.GetEnvironmentVariable("RENDER") == "true" 
    ? "/tmp/minblog.db" 
    : "minblog.db";      

Console.WriteLine($"Путь к БД: {dbPath}");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPostService, PostService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        bool created = dbContext.Database.EnsureCreated();
        Console.WriteLine(created ? "База данных и таблицы созданы" : "База данных уже существовала");
        
        var connection = dbContext.Database.GetDbConnection();
        await connection.OpenAsync();
        
        using var checkCommand = connection.CreateCommand();
        checkCommand.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='Posts';";
        var tableExists = await checkCommand.ExecuteScalarAsync();
        
        if (tableExists == null)
        {
            Console.WriteLine("Таблица Posts не найдена, создаём вручную...");
            
            using var createCommand = connection.CreateCommand();
            createCommand.CommandText = @"
                CREATE TABLE Posts (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Content TEXT NOT NULL,
                    Author TEXT,
                    CreatedAt TEXT NOT NULL
                );";
            await createCommand.ExecuteNonQueryAsync();
            Console.WriteLine("Таблица Posts создана вручную");
        }
        else
        {
            Console.WriteLine("Таблица Posts существует, всё хорошо");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при создании таблиц: {ex.Message}");
    }
}

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
