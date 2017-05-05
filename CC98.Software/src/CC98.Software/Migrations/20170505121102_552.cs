using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CC98.Software.Migrations
{
    public partial class _552 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Softwares_CommentBelongtoId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Comments",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Contents",
                table: "Comments",
                newName: "Content");

            migrationBuilder.RenameColumn(
                name: "Commenttime",
                table: "Comments",
                newName: "Time");

            migrationBuilder.RenameColumn(
                name: "CommentBelongtoId",
                table: "Comments",
                newName: "SoftwareId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentBelongtoId",
                table: "Comments",
                newName: "IX_Comments_SoftwareId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Softwares_SoftwareId",
                table: "Comments",
                column: "SoftwareId",
                principalTable: "Softwares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Softwares_SoftwareId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Comments",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "Comments",
                newName: "Commenttime");

            migrationBuilder.RenameColumn(
                name: "SoftwareId",
                table: "Comments",
                newName: "CommentBelongtoId");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Comments",
                newName: "Contents");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_SoftwareId",
                table: "Comments",
                newName: "IX_Comments_CommentBelongtoId");

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Softwares_CommentBelongtoId",
                table: "Comments",
                column: "CommentBelongtoId",
                principalTable: "Softwares",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
