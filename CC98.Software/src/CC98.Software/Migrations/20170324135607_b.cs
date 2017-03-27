using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CC98.Software.Migrations
{
    public partial class b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RecieverName",
                table: "Feedbacks",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverName",
                table: "Feedbacks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverName",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Feedbacks",
                newName: "RecieverName");
        }
    }
}
