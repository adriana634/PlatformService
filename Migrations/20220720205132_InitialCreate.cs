﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cost = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "Id", "Cost", "Name", "Publisher" },
                values: new object[] { 1, "Free", "Dot Net", "Microsoft" });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "Id", "Cost", "Name", "Publisher" },
                values: new object[] { 2, "Free", "SQL Server Express", "Microsoft" });

            migrationBuilder.InsertData(
                table: "Platforms",
                columns: new[] { "Id", "Cost", "Name", "Publisher" },
                values: new object[] { 3, "Free", "Kubernetes", "Cloud Native Computing Foundation" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Platforms");
        }
    }
}
