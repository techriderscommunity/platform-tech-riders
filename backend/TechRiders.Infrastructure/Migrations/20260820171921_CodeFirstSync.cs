using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechRiders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CodeFirstSync : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_candidaturas_ofertas_oferta_id",
                schema: "dbo",
                table: "candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_FPTours_Ambassadors_AmbassadorId",
                table: "FPTours");

            migrationBuilder.DropTable(
                name: "intranet_audit_logs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "intranet_settings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "intranet_user_categories",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_EventId_StartTime",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Events_StartDate",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_StartDate_EndDate",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tutoriales",
                schema: "dbo",
                table: "tutoriales");

            migrationBuilder.DropIndex(
                name: "IX_tutoriales_autor",
                schema: "dbo",
                table: "tutoriales");

            migrationBuilder.DropIndex(
                name: "IX_tutoriales_is_active",
                schema: "dbo",
                table: "tutoriales");

            migrationBuilder.DropIndex(
                name: "IX_tutoriales_slug_unique",
                schema: "dbo",
                table: "tutoriales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ofertas",
                schema: "dbo",
                table: "ofertas");

            migrationBuilder.DropIndex(
                name: "IX_ofertas_estado_fecha",
                schema: "dbo",
                table: "ofertas");

            migrationBuilder.DropIndex(
                name: "IX_ofertas_is_active",
                schema: "dbo",
                table: "ofertas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_candidaturas",
                schema: "dbo",
                table: "candidaturas");

            migrationBuilder.DropIndex(
                name: "IX_candidaturas_is_active",
                schema: "dbo",
                table: "candidaturas");

            migrationBuilder.DropIndex(
                name: "IX_candidaturas_junior_id",
                schema: "dbo",
                table: "candidaturas");

            migrationBuilder.DropIndex(
                name: "IX_candidaturas_oferta_id",
                schema: "dbo",
                table: "candidaturas");

            migrationBuilder.DropIndex(
                name: "IX_candidaturas_oferta_junior_unique",
                schema: "dbo",
                table: "candidaturas");

            migrationBuilder.DropColumn(
                name: "HasContactAmbassador",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "HasContactCenter",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "HasDeliveredAmbassador",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "HasDeliveredCenter",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "HasFeedbackAmbassador",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "HasFeedbackCenter",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "HasPhotosAmbassador",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "HasPhotosCenter",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "NumberStudents",
                table: "Centers");

            migrationBuilder.DropColumn(
                name: "row_version",
                schema: "dbo",
                table: "tutoriales");

            migrationBuilder.DropColumn(
                name: "row_version",
                schema: "dbo",
                table: "ofertas");

            migrationBuilder.DropColumn(
                name: "row_version",
                schema: "dbo",
                table: "candidaturas");

            migrationBuilder.RenameTable(
                name: "tutoriales",
                schema: "dbo",
                newName: "Tutoriales");

            migrationBuilder.RenameTable(
                name: "ofertas",
                schema: "dbo",
                newName: "Ofertas");

            migrationBuilder.RenameTable(
                name: "candidaturas",
                schema: "dbo",
                newName: "Candidaturas");

            migrationBuilder.RenameColumn(
                name: "Studies",
                table: "Centers",
                newName: "ParkingInfo");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "Tutoriales",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "titulo",
                table: "Tutoriales",
                newName: "Titulo");

            migrationBuilder.RenameColumn(
                name: "slug",
                table: "Tutoriales",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "extracto",
                table: "Tutoriales",
                newName: "Extracto");

            migrationBuilder.RenameColumn(
                name: "autor",
                table: "Tutoriales",
                newName: "Autor");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Tutoriales",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Tutoriales",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Tutoriales",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "fecha_publicacion",
                table: "Tutoriales",
                newName: "FechaPublicacion");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Tutoriales",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "categorias_json",
                table: "Tutoriales",
                newName: "CategoriasJson");

            migrationBuilder.RenameColumn(
                name: "ubicacion",
                table: "Ofertas",
                newName: "Ubicacion");

            migrationBuilder.RenameColumn(
                name: "titulo",
                table: "Ofertas",
                newName: "Titulo");

            migrationBuilder.RenameColumn(
                name: "salario",
                table: "Ofertas",
                newName: "Salario");

            migrationBuilder.RenameColumn(
                name: "modalidad",
                table: "Ofertas",
                newName: "Modalidad");

            migrationBuilder.RenameColumn(
                name: "estado",
                table: "Ofertas",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "empresa",
                table: "Ofertas",
                newName: "Empresa");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Ofertas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Ofertas",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Ofertas",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "fecha_publicacion",
                table: "Ofertas",
                newName: "FechaPublicacion");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Ofertas",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "estado",
                table: "Candidaturas",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Candidaturas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Candidaturas",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "oferta_id",
                table: "Candidaturas",
                newName: "OfertaId");

            migrationBuilder.RenameColumn(
                name: "nombre_junior",
                table: "Candidaturas",
                newName: "NombreJunior");

            migrationBuilder.RenameColumn(
                name: "junior_id",
                table: "Candidaturas",
                newName: "JuniorId");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Candidaturas",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "fecha_solicitud",
                table: "Candidaturas",
                newName: "FechaSolicitud");

            migrationBuilder.RenameColumn(
                name: "email_junior",
                table: "Candidaturas",
                newName: "EmailJunior");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Candidaturas",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<Guid>(
                name: "CenterId",
                table: "Sessions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndDateTime",
                table: "Sessions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDateTime",
                table: "Sessions",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "Sessions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentCount",
                table: "Sessions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AmbassadorId1",
                table: "FPTours",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AmbassadorUserId",
                table: "FPTours",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "FPTours",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PlannedDate",
                table: "FPTours",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "FPTours",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Events",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "CenterId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndDateTime",
                table: "Events",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EventTypeId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDateTime",
                table: "Events",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "Events",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Events",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Centers",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Ambassadors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Tutoriales",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Tutoriales",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Tutoriales",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Extracto",
                table: "Tutoriales",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Autor",
                table: "Tutoriales",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Tutoriales",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoriasJson",
                table: "Tutoriales",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldDefaultValue: "[]");

            migrationBuilder.AlterColumn<string>(
                name: "Ubicacion",
                table: "Ofertas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Ofertas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<decimal>(
                name: "Salario",
                table: "Ofertas",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "Modalidad",
                table: "Ofertas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "Ofertas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Empresa",
                table: "Ofertas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Ofertas",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<int>(
                name: "Estado",
                table: "Candidaturas",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NombreJunior",
                table: "Candidaturas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "JuniorId",
                table: "Candidaturas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Candidaturas",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "EmailJunior",
                table: "Candidaturas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tutoriales",
                table: "Tutoriales",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ofertas",
                table: "Ofertas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Candidaturas",
                table: "Candidaturas",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CenterStudies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CenterStudies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CenterStudies_Centers_CenterId",
                        column: x => x.CenterId,
                        principalTable: "Centers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FPTourTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FPTourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FPTourTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FPTourTasks_FPTours_FPTourId",
                        column: x => x.FPTourId,
                        principalTable: "FPTours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntranetAuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntranetAuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntranetSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Module = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntranetSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntranetUserCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntranetUserCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentSkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_Skills_ParentSkillId",
                        column: x => x.ParentSkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Scope = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventCategories",
                columns: table => new
                {
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => new { x.EventId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_EventCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventCategories_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionCategories",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionCategories", x => new { x.SessionId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_SessionCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionCategories_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionSkills",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionSkills", x => new { x.SessionId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_SessionSkills_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Locality = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    IsWorking = table.Column<bool>(type: "bit", nullable: false),
                    LastActivityDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    GPFId = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    About = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Github = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CenterContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CenterContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CenterContacts_Centers_CenterId",
                        column: x => x.CenterId,
                        principalTable: "Centers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CenterContacts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Communities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ContactUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communities_Users_ContactUserId",
                        column: x => x.ContactUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ContactUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Companies_Users_ContactUserId",
                        column: x => x.ContactUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "EventRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationStatus = table.Column<int>(type: "int", nullable: false),
                    Attended = table.Column<bool>(type: "bit", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventRegistrations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeArticles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    ContentMd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AuthorUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KnowledgeArticles_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KnowledgeArticles_Users_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SessionRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationStatus = table.Column<int>(type: "int", nullable: false),
                    Attended = table.Column<bool>(type: "bit", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionRegistrations_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionRegistrations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SessionSpeakers",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsMainSpeaker = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionSpeakers", x => new { x.SessionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SessionSpeakers_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionSpeakers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserCategories",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCategories", x => new { x.UserId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_UserCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCategories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkills",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    IsSpeakerSkill = table.Column<bool>(type: "bit", nullable: false),
                    IsMentorSkill = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkills", x => new { x.UserId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_UserSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkills_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommunityCollaborations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CommunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityCollaborations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityCollaborations_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityCollaborations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CommunityMembers",
                columns: table => new
                {
                    CommunityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMembers", x => new { x.CommunityId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CommunityMembers_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityMembers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    ContractType = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PublishedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ClosingAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
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
                name: "KnowledgeArticleCategories",
                columns: table => new
                {
                    KnowledgeArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeArticleCategories", x => new { x.KnowledgeArticleId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_KnowledgeArticleCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KnowledgeArticleCategories_KnowledgeArticles_KnowledgeArticleId",
                        column: x => x.KnowledgeArticleId,
                        principalTable: "KnowledgeArticles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KnowledgeArticleSkills",
                columns: table => new
                {
                    KnowledgeArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KnowledgeArticleSkills", x => new { x.KnowledgeArticleId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_KnowledgeArticleSkills_KnowledgeArticles_KnowledgeArticleId",
                        column: x => x.KnowledgeArticleId,
                        principalTable: "KnowledgeArticles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KnowledgeArticleSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CenterId",
                table: "Sessions",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_EventId_StartDateTime",
                table: "Sessions",
                columns: new[] { "EventId", "StartDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_StatusId",
                table: "Sessions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_FPTours_AmbassadorId1",
                table: "FPTours",
                column: "AmbassadorId1");

            migrationBuilder.CreateIndex(
                name: "IX_FPTours_StatusId",
                table: "FPTours",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CenterId",
                table: "Events",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventTypeId",
                table: "Events",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartDateTime",
                table: "Events",
                column: "StartDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartDateTime_EndDateTime",
                table: "Events",
                columns: new[] { "StartDateTime", "EndDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_StatusId",
                table: "Events",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CenterContacts_CenterId",
                table: "CenterContacts",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_CenterContacts_UserId",
                table: "CenterContacts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CenterStudies_CenterId",
                table: "CenterStudies",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Communities_ContactUserId",
                table: "Communities",
                column: "ContactUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityCollaborations_CommunityId",
                table: "CommunityCollaborations",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityCollaborations_EventId",
                table: "CommunityCollaborations",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMembers_UserId",
                table: "CommunityMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ContactUserId",
                table: "Companies",
                column: "ContactUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventCategories_CategoryId",
                table: "EventCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_EventId_UserId",
                table: "EventRegistrations",
                columns: new[] { "EventId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventRegistrations_UserId",
                table: "EventRegistrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTypes_Name",
                table: "EventTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FPTourTasks_FPTourId",
                table: "FPTourTasks",
                column: "FPTourId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_CategoryId",
                table: "JobOffers",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_CompanyId",
                table: "JobOffers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeArticleCategories_CategoryId",
                table: "KnowledgeArticleCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeArticles_AuthorUserId",
                table: "KnowledgeArticles",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeArticles_Slug",
                table: "KnowledgeArticles",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeArticles_StatusId",
                table: "KnowledgeArticles",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_KnowledgeArticleSkills_SkillId",
                table: "KnowledgeArticleSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionCategories_CategoryId",
                table: "SessionCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionRegistrations_SessionId_UserId",
                table: "SessionRegistrations",
                columns: new[] { "SessionId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SessionRegistrations_UserId",
                table: "SessionRegistrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionSkills_SkillId",
                table: "SessionSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionSpeakers_UserId",
                table: "SessionSpeakers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ParentSkillId",
                table: "Skills",
                column: "ParentSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCategories_CategoryId",
                table: "UserCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_StatusId",
                table: "Users",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_SkillId",
                table: "UserSkills",
                column: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Centers_CenterId",
                table: "Events",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventTypes_EventTypeId",
                table: "Events",
                column: "EventTypeId",
                principalTable: "EventTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Statuses_StatusId",
                table: "Events",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FPTours_Ambassadors_AmbassadorId1",
                table: "FPTours",
                column: "AmbassadorId1",
                principalTable: "Ambassadors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FPTours_Statuses_StatusId",
                table: "FPTours",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FPTours_Users_AmbassadorId",
                table: "FPTours",
                column: "AmbassadorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Centers_CenterId",
                table: "Sessions",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Statuses_StatusId",
                table: "Sessions",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Centers_CenterId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventTypes_EventTypeId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Statuses_StatusId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_FPTours_Ambassadors_AmbassadorId1",
                table: "FPTours");

            migrationBuilder.DropForeignKey(
                name: "FK_FPTours_Statuses_StatusId",
                table: "FPTours");

            migrationBuilder.DropForeignKey(
                name: "FK_FPTours_Users_AmbassadorId",
                table: "FPTours");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Centers_CenterId",
                table: "Sessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Statuses_StatusId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "CenterContacts");

            migrationBuilder.DropTable(
                name: "CenterStudies");

            migrationBuilder.DropTable(
                name: "CommunityCollaborations");

            migrationBuilder.DropTable(
                name: "CommunityMembers");

            migrationBuilder.DropTable(
                name: "EventCategories");

            migrationBuilder.DropTable(
                name: "EventRegistrations");

            migrationBuilder.DropTable(
                name: "EventTypes");

            migrationBuilder.DropTable(
                name: "FPTourTasks");

            migrationBuilder.DropTable(
                name: "IntranetAuditLogs");

            migrationBuilder.DropTable(
                name: "IntranetSettings");

            migrationBuilder.DropTable(
                name: "IntranetUserCategories");

            migrationBuilder.DropTable(
                name: "JobOffers");

            migrationBuilder.DropTable(
                name: "KnowledgeArticleCategories");

            migrationBuilder.DropTable(
                name: "KnowledgeArticleSkills");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "SessionCategories");

            migrationBuilder.DropTable(
                name: "SessionRegistrations");

            migrationBuilder.DropTable(
                name: "SessionSkills");

            migrationBuilder.DropTable(
                name: "SessionSpeakers");

            migrationBuilder.DropTable(
                name: "UserCategories");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserSkills");

            migrationBuilder.DropTable(
                name: "Communities");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "KnowledgeArticles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_CenterId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_EventId_StartDateTime",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_Sessions_StatusId",
                table: "Sessions");

            migrationBuilder.DropIndex(
                name: "IX_FPTours_AmbassadorId1",
                table: "FPTours");

            migrationBuilder.DropIndex(
                name: "IX_FPTours_StatusId",
                table: "FPTours");

            migrationBuilder.DropIndex(
                name: "IX_Events_CenterId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventTypeId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_StartDateTime",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_StartDateTime_EndDateTime",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_StatusId",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tutoriales",
                table: "Tutoriales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ofertas",
                table: "Ofertas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Candidaturas",
                table: "Candidaturas");

            migrationBuilder.DropColumn(
                name: "CenterId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "StudentCount",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "AmbassadorId1",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "AmbassadorUserId",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "PlannedDate",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "FPTours");

            migrationBuilder.DropColumn(
                name: "CenterId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventTypeId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StartDateTime",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Centers");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "Tutoriales",
                newName: "tutoriales",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Ofertas",
                newName: "ofertas",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "Candidaturas",
                newName: "candidaturas",
                newSchema: "dbo");

            migrationBuilder.RenameColumn(
                name: "ParkingInfo",
                table: "Centers",
                newName: "Studies");

            migrationBuilder.RenameColumn(
                name: "Url",
                schema: "dbo",
                table: "tutoriales",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Titulo",
                schema: "dbo",
                table: "tutoriales",
                newName: "titulo");

            migrationBuilder.RenameColumn(
                name: "Slug",
                schema: "dbo",
                table: "tutoriales",
                newName: "slug");

            migrationBuilder.RenameColumn(
                name: "Extracto",
                schema: "dbo",
                table: "tutoriales",
                newName: "extracto");

            migrationBuilder.RenameColumn(
                name: "Autor",
                schema: "dbo",
                table: "tutoriales",
                newName: "autor");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "tutoriales",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "dbo",
                table: "tutoriales",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "dbo",
                table: "tutoriales",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "FechaPublicacion",
                schema: "dbo",
                table: "tutoriales",
                newName: "fecha_publicacion");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "tutoriales",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CategoriasJson",
                schema: "dbo",
                table: "tutoriales",
                newName: "categorias_json");

            migrationBuilder.RenameColumn(
                name: "Ubicacion",
                schema: "dbo",
                table: "ofertas",
                newName: "ubicacion");

            migrationBuilder.RenameColumn(
                name: "Titulo",
                schema: "dbo",
                table: "ofertas",
                newName: "titulo");

            migrationBuilder.RenameColumn(
                name: "Salario",
                schema: "dbo",
                table: "ofertas",
                newName: "salario");

            migrationBuilder.RenameColumn(
                name: "Modalidad",
                schema: "dbo",
                table: "ofertas",
                newName: "modalidad");

            migrationBuilder.RenameColumn(
                name: "Estado",
                schema: "dbo",
                table: "ofertas",
                newName: "estado");

            migrationBuilder.RenameColumn(
                name: "Empresa",
                schema: "dbo",
                table: "ofertas",
                newName: "empresa");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "ofertas",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "dbo",
                table: "ofertas",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "dbo",
                table: "ofertas",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "FechaPublicacion",
                schema: "dbo",
                table: "ofertas",
                newName: "fecha_publicacion");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "ofertas",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Estado",
                schema: "dbo",
                table: "candidaturas",
                newName: "estado");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "dbo",
                table: "candidaturas",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "dbo",
                table: "candidaturas",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OfertaId",
                schema: "dbo",
                table: "candidaturas",
                newName: "oferta_id");

            migrationBuilder.RenameColumn(
                name: "NombreJunior",
                schema: "dbo",
                table: "candidaturas",
                newName: "nombre_junior");

            migrationBuilder.RenameColumn(
                name: "JuniorId",
                schema: "dbo",
                table: "candidaturas",
                newName: "junior_id");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "dbo",
                table: "candidaturas",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "FechaSolicitud",
                schema: "dbo",
                table: "candidaturas",
                newName: "fecha_solicitud");

            migrationBuilder.RenameColumn(
                name: "EmailJunior",
                schema: "dbo",
                table: "candidaturas",
                newName: "email_junior");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "dbo",
                table: "candidaturas",
                newName: "created_at");

            migrationBuilder.AddColumn<bool>(
                name: "HasContactAmbassador",
                table: "FPTours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasContactCenter",
                table: "FPTours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasDeliveredAmbassador",
                table: "FPTours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasDeliveredCenter",
                table: "FPTours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasFeedbackAmbassador",
                table: "FPTours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasFeedbackCenter",
                table: "FPTours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPhotosAmbassador",
                table: "FPTours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPhotosCenter",
                table: "FPTours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberStudents",
                table: "Centers",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nickname",
                table: "Ambassadors",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "url",
                schema: "dbo",
                table: "tutoriales",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "titulo",
                schema: "dbo",
                table: "tutoriales",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                schema: "dbo",
                table: "tutoriales",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "extracto",
                schema: "dbo",
                table: "tutoriales",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "autor",
                schema: "dbo",
                table: "tutoriales",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                schema: "dbo",
                table: "tutoriales",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "categorias_json",
                schema: "dbo",
                table: "tutoriales",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "[]",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<byte[]>(
                name: "row_version",
                schema: "dbo",
                table: "tutoriales",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "ubicacion",
                schema: "dbo",
                table: "ofertas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "titulo",
                schema: "dbo",
                table: "ofertas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "salario",
                schema: "dbo",
                table: "ofertas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<string>(
                name: "modalidad",
                schema: "dbo",
                table: "ofertas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "estado",
                schema: "dbo",
                table: "ofertas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "empresa",
                schema: "dbo",
                table: "ofertas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                schema: "dbo",
                table: "ofertas",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<byte[]>(
                name: "row_version",
                schema: "dbo",
                table: "ofertas",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "estado",
                schema: "dbo",
                table: "candidaturas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "nombre_junior",
                schema: "dbo",
                table: "candidaturas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "junior_id",
                schema: "dbo",
                table: "candidaturas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                schema: "dbo",
                table: "candidaturas",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "email_junior",
                schema: "dbo",
                table: "candidaturas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<byte[]>(
                name: "row_version",
                schema: "dbo",
                table: "candidaturas",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddPrimaryKey(
                name: "PK_tutoriales",
                schema: "dbo",
                table: "tutoriales",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ofertas",
                schema: "dbo",
                table: "ofertas",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_candidaturas",
                schema: "dbo",
                table: "candidaturas",
                column: "id");

            migrationBuilder.CreateTable(
                name: "intranet_audit_logs",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    action = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    actor_email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    actor_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    detail = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    module = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    result = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_intranet_audit_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "intranet_settings",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    module = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "activo"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    updated_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    value = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_intranet_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "intranet_user_categories",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    category = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_intranet_user_categories", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_EventId_StartTime",
                table: "Sessions",
                columns: new[] { "EventId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartDate",
                table: "Events",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartDate_EndDate",
                table: "Events",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_tutoriales_autor",
                schema: "dbo",
                table: "tutoriales",
                column: "autor");

            migrationBuilder.CreateIndex(
                name: "IX_tutoriales_is_active",
                schema: "dbo",
                table: "tutoriales",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_tutoriales_slug_unique",
                schema: "dbo",
                table: "tutoriales",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ofertas_estado_fecha",
                schema: "dbo",
                table: "ofertas",
                columns: new[] { "estado", "fecha_publicacion" });

            migrationBuilder.CreateIndex(
                name: "IX_ofertas_is_active",
                schema: "dbo",
                table: "ofertas",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_candidaturas_is_active",
                schema: "dbo",
                table: "candidaturas",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_candidaturas_junior_id",
                schema: "dbo",
                table: "candidaturas",
                column: "junior_id");

            migrationBuilder.CreateIndex(
                name: "IX_candidaturas_oferta_id",
                schema: "dbo",
                table: "candidaturas",
                column: "oferta_id");

            migrationBuilder.CreateIndex(
                name: "IX_candidaturas_oferta_junior_unique",
                schema: "dbo",
                table: "candidaturas",
                columns: new[] { "oferta_id", "junior_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_intranet_audit_logs_actor_user_id",
                schema: "dbo",
                table: "intranet_audit_logs",
                column: "actor_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_intranet_audit_logs_created_utc",
                schema: "dbo",
                table: "intranet_audit_logs",
                column: "created_utc");

            migrationBuilder.CreateIndex(
                name: "IX_intranet_audit_logs_module_action",
                schema: "dbo",
                table: "intranet_audit_logs",
                columns: new[] { "module", "action" });

            migrationBuilder.CreateIndex(
                name: "IX_intranet_settings_key_unique",
                schema: "dbo",
                table: "intranet_settings",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_intranet_settings_module",
                schema: "dbo",
                table: "intranet_settings",
                column: "module");

            migrationBuilder.CreateIndex(
                name: "IX_intranet_settings_status",
                schema: "dbo",
                table: "intranet_settings",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_intranet_user_categories_active",
                schema: "dbo",
                table: "intranet_user_categories",
                column: "active");

            migrationBuilder.CreateIndex(
                name: "IX_intranet_user_categories_user_category_unique",
                schema: "dbo",
                table: "intranet_user_categories",
                columns: new[] { "user_id", "category" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_intranet_user_categories_user_id",
                schema: "dbo",
                table: "intranet_user_categories",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_candidaturas_ofertas_oferta_id",
                schema: "dbo",
                table: "candidaturas",
                column: "oferta_id",
                principalSchema: "dbo",
                principalTable: "ofertas",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FPTours_Ambassadors_AmbassadorId",
                table: "FPTours",
                column: "AmbassadorId",
                principalTable: "Ambassadors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
