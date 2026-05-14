using Microsoft.EntityFrameworkCore;
using MiniBlog.Models;

namespace MiniBlog.Data
{
    public class ApplicationDbContext : DbContext
    {
        // конструктор принимает настройки из Program.cs
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet представляет таблицу в БД и ентити сам все определит
        public DbSet<Post> Posts { get; set; }
    }
}