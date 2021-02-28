using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VS_SLG6.Model.Migrations
{
    public partial class Message_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Message",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ReceiptId",
                table: "Message",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "Message",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Message");
        }
    }
}
