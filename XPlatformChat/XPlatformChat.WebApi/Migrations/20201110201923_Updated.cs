using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace XPlatformChat.WebApi.Migrations
{
    public partial class Updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Edited",
                table: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Messages");

            migrationBuilder.AddColumn<DateTime>(
                name: "Edited",
                table: "Messages",
                type: "datetime2",
                nullable: true);
        }
    }
}
