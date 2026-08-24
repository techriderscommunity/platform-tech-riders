using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechRiders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CodeFirstSyncFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FPTours_Ambassadors_AmbassadorId1",
                table: "FPTours");

            migrationBuilder.DropForeignKey(
                name: "FK_FPTours_Users_AmbassadorId",
                table: "FPTours");

            migrationBuilder.DropIndex(
                name: "IX_FPTours_AmbassadorId",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "AmbassadorId",
                table: "FPTours");

            migrationBuilder.AlterColumn<Guid>(
                name: "AmbassadorUserId",
                table: "FPTours",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AmbassadorId1",
                table: "FPTours",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_FPTours_AmbassadorUserId",
                table: "FPTours",
                column: "AmbassadorUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FPTours_Ambassadors_AmbassadorId1",
                table: "FPTours",
                column: "AmbassadorId1",
                principalTable: "Ambassadors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FPTours_Users_AmbassadorUserId",
                table: "FPTours",
                column: "AmbassadorUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FPTours_Ambassadors_AmbassadorId1",
                table: "FPTours");

            migrationBuilder.DropForeignKey(
                name: "FK_FPTours_Users_AmbassadorUserId",
                table: "FPTours");

            migrationBuilder.DropIndex(
                name: "IX_FPTours_AmbassadorUserId",
                table: "FPTours");

            migrationBuilder.AlterColumn<Guid>(
                name: "AmbassadorUserId",
                table: "FPTours",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "AmbassadorId1",
                table: "FPTours",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AmbassadorId",
                table: "FPTours",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_FPTours_AmbassadorId",
                table: "FPTours",
                column: "AmbassadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FPTours_Ambassadors_AmbassadorId1",
                table: "FPTours",
                column: "AmbassadorId1",
                principalTable: "Ambassadors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FPTours_Users_AmbassadorId",
                table: "FPTours",
                column: "AmbassadorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
