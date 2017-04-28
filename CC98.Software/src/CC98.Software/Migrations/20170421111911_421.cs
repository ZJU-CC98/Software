using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CC98.Software.Migrations
{
    public partial class _421 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SMessages");

            migrationBuilder.AddColumn<string>(
                name: "Uploadername",
                table: "Softwares",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isRecommended",
                table: "Softwares",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Commenttime",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uploadername",
                table: "Softwares");

            migrationBuilder.DropColumn(
                name: "isRecommended",
                table: "Softwares");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Commenttime",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.CreateTable(
                name: "SMessages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    Receivername = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SMessages", x => x.Id);
                });
        }
    }
}
