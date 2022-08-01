using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task5WebApp.Migrations
{
    public partial class SenderAndDataAdding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "Message",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOfSending",
                table: "Message",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "TimeOfSending",
                table: "Message");
        }
    }
}
