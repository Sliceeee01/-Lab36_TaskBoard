using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using TaskBoardApi.Models;

namespace TaskBoardApi.Data;
public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<TaskItem> Tasks { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<TaskItem>().HasData(
new TaskItem {
    id = 1,
    Title = "Изучить ASP.NET Core",
    Description = "Контролеры , маршруты, middleware",
    IsCompleted = true,
    CreateAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
},
new TaskItem {
    id = 2,
    Title = "Подключить SQLite",
    Description = "EF Core,миграции, DbContext",
    IsCompleted = true,
    CreateAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc)
},
new TaskItem {
    id = 3,
    Title = "Написать frontend",
    Description = "HTML, Css ,JavaScript, fetch",
    IsCompleted = false,
    CreateAt = new DateTime(2026, 1, 3, 0, 0, 0, DateTimeKind.Utc)
},
new TaskItem {
    id = 4,
    Title = "Сдать итоговый проект",
    Description = "Показать преподавателю работающее приложение",
    IsCompleted = false,
    CreateAt = new DateTime(2026, 1, 4, 0, 0, 0, DateTimeKind.Utc)
}
);
    }
}