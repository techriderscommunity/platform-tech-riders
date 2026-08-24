using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechRiders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncModelWithContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ambassadors");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Tutoriales");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Tutoriales");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Ofertas");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Ofertas");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "IntranetUserCategories");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "IntranetUserCategories");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "IntranetSettings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "IntranetSettings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "IntranetSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "IntranetSettings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "IntranetAuditLogs");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "IntranetAuditLogs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "IntranetAuditLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "IntranetAuditLogs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Candidaturas");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Candidaturas");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "Locality",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkedIn",
                table: "Users",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(160)",
                oldMaxLength: 160);

            migrationBuilder.AlterColumn<string>(
                name: "Instagram",
                table: "Users",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Github",
                table: "Users",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "MT_Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MT_Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "MT_Categories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "IntranetSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "IntranetSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 201,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 202,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 203,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 204,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 301,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 302,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 303,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 401,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 402,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 403,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 501,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 502,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "MT_Categories",
                keyColumn: "Id",
                keyValue: 503,
                columns: new[] { "Color", "Description", "Icon" },
                values: new object[] { null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsWorking",
                table: "Users",
                column: "IsWorking");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_IsWorking",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "MT_Categories");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MT_Categories");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MT_Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Users",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Users",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Locality",
                table: "Users",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkedIn",
                table: "Users",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Instagram",
                table: "Users",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Github",
                table: "Users",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Tutoriales",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tutoriales",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Ofertas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Ofertas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "IntranetUserCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "IntranetUserCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "IntranetSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "IntranetSettings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "IntranetSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "IntranetSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "IntranetSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "IntranetSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "IntranetAuditLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "IntranetAuditLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "IntranetAuditLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "IntranetAuditLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Candidaturas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Candidaturas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ambassadors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    About = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Github = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsWorking = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LinkedIn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Locality = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OtherCategory = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Skill = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ambassadors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ambassadors_MT_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "MT_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ambassadors_CategoryId",
                table: "Ambassadors",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Ambassadors_Email",
                table: "Ambassadors",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Ambassadors_IsActive",
                table: "Ambassadors",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Ambassadors_IsWorking",
                table: "Ambassadors",
                column: "IsWorking");
        }
    }
}
