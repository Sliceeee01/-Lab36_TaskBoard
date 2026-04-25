using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskBoardApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "id", "CreateAt", "Description", "IsCompleted", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Контролеры , маршруты, middleware", true, "Изучить ASP.NET Core" },
                    { 2, new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "EF Core,миграции, DbContext", true, "Подключить SQLite" },
                    { 3, new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), "HTML, Css ,JavaScript, fetch", false, "Написать frontend" },
                    { 4, new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Показать преподавателю работающее приложение", false, "Сдать итоговый проект" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
