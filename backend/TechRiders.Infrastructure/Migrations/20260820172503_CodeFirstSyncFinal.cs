using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechRiders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CodeFirstSyncFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FPTours_Ambassadors_AmbassadorId1",
                table: "FPTours");

            migrationBuilder.DropIndex(
                name: "IX_FPTours_AmbassadorId1",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "AmbassadorId1",
                table: "FPTours");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AmbassadorId1",
                table: "FPTours",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FPTours_AmbassadorId1",
                table: "FPTours",
                column: "AmbassadorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FPTours_Ambassadors_AmbassadorId1",
                table: "FPTours",
                column: "AmbassadorId1",
                principalTable: "Ambassadors",
                principalColumn: "Id");
        }
    }
}
