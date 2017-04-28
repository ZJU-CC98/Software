using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CC98.Software.Migrations
{
    public partial class _4212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isRecommended",
                table: "Softwares",
                newName: "IsRecommended");

            migrationBuilder.RenameColumn(
                name: "Uploadername",
                table: "Softwares",
                newName: "UploaderName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UploaderName",
                table: "Softwares",
                newName: "Uploadername");

            migrationBuilder.RenameColumn(
                name: "IsRecommended",
                table: "Softwares",
                newName: "isRecommended");
        }
    }
}
