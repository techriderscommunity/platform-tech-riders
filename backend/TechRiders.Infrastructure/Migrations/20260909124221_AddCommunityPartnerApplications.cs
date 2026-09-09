using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechRiders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunityPartnerApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidaturas");

            migrationBuilder.DropTable(
                name: "JobOffers");

            migrationBuilder.DropTable(
                name: "Ofertas");

            migrationBuilder.DropTable(
                name: "Tutoriales");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "MT_Categories");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MT_Categories");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "MT_Categories");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "IntranetUserCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "IntranetSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "IntranetSettings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "ActorUserId",
                table: "IntranetAuditLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "ActorEmail",
                table: "IntranetAuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "IntranetAuditLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Detail",
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

            migrationBuilder.CreateTable(
                name: "CommunityPartnerApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Website = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WhoYouAre = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    WhatYouDo = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Mission = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Topics = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LinkedIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    X = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YouTube = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discord = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telegram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Motivation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollaborationIdeas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, defaultValue: "pending"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityPartnerApplications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPartnerApplications_ContactEmail",
                table: "CommunityPartnerApplications",
                column: "ContactEmail");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPartnerApplications_Name",
                table: "CommunityPartnerApplications",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPartnerApplications_Status",
                table: "CommunityPartnerApplications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityPartnerApplications_Website",
                table: "CommunityPartnerApplications",
                column: "Website");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityPartnerApplications");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "IntranetUserCategories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "IntranetSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "IntranetSettings");

            migrationBuilder.DropColumn(
                name: "ActorEmail",
                table: "IntranetAuditLogs");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "IntranetAuditLogs");

            migrationBuilder.DropColumn(
                name: "Detail",
                table: "IntranetAuditLogs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "IntranetAuditLogs");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "IntranetAuditLogs");

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

            migrationBuilder.AlterColumn<Guid>(
                name: "ActorUserId",
                table: "IntranetAuditLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Candidaturas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailJunior = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    JuniorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreJunior = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfertaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidaturas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClosingAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ContractType = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    PublishedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOffers_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_JobOffers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ofertas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Modalidad = table.Column<int>(type: "int", nullable: false),
                    Requisitos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofertas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tutoriales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoriasJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Extracto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tutoriales", x => x.Id);
                });

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
                name: "IX_JobOffers_CategoryId",
                table: "JobOffers",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_CompanyId",
                table: "JobOffers",
                column: "CompanyId");
        }
    }
}
