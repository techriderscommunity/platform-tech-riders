using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TechRiders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Centers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Locality = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Studies = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Specialty = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NumberStudents = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Parking = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Centers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MaxCapacity = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "intranet_audit_logs",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    actor_user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    actor_email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    module = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    action = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    result = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    detail = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
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
                    key = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    module = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    value = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "activo"),
                    updated_utc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_by = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
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
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    category = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_intranet_user_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MT_Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FatherId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MT_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MT_Categories_MT_Categories_FatherId",
                        column: x => x.FatherId,
                        principalTable: "MT_Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ofertas",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    empresa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    salario = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ubicacion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requisitos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    modalidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    estado = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    fecha_publicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ofertas", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tutoriales",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    slug = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    extracto = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    autor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    fecha_publicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    categorias_json = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false, defaultValue: "[]"),
                    url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tutoriales", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Speaker = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Room = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaxCapacity = table.Column<int>(type: "int", nullable: true),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ambassadors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Locality = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsWorking = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    OtherCategory = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    About = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Skill = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Github = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
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

            migrationBuilder.CreateTable(
                name: "candidaturas",
                schema: "dbo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    oferta_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    junior_id = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    nombre_junior = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email_junior = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fecha_solicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    row_version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_candidaturas", x => x.id);
                    table.ForeignKey(
                        name: "FK_candidaturas_ofertas_oferta_id",
                        column: x => x.oferta_id,
                        principalSchema: "dbo",
                        principalTable: "ofertas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FPTours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmbassadorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HasContactCenter = table.Column<bool>(type: "bit", nullable: false),
                    HasContactAmbassador = table.Column<bool>(type: "bit", nullable: false),
                    HasScheduledDate = table.Column<bool>(type: "bit", nullable: false),
                    HasFeedbackCenter = table.Column<bool>(type: "bit", nullable: false),
                    HasFeedbackAmbassador = table.Column<bool>(type: "bit", nullable: false),
                    HasPhotosCenter = table.Column<bool>(type: "bit", nullable: false),
                    HasPhotosAmbassador = table.Column<bool>(type: "bit", nullable: false),
                    HasDeliveredCenter = table.Column<bool>(type: "bit", nullable: false),
                    HasDeliveredAmbassador = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FPTours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FPTours_Ambassadors_AmbassadorId",
                        column: x => x.AmbassadorId,
                        principalTable: "Ambassadors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FPTours_Centers_CenterId",
                        column: x => x.CenterId,
                        principalTable: "Centers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MT_Categories",
                columns: new[] { "Id", "Active", "FatherId", "Name" },
                values: new object[,]
                {
                    { 1, true, null, "Desarrollo y Programación Software" },
                    { 2, true, null, "Sistemas, Redes e Infraestructura" },
                    { 3, true, null, "Datos e Inteligencia Artificial" },
                    { 4, true, null, "Diseño y Gestión Digital" },
                    { 5, true, null, "Habilidades y Orientación Laboral" },
                    { 101, true, 1, "Programación Frontend" },
                    { 102, true, 1, "Programación Backend" },
                    { 103, true, 1, "Desarrollo Móvil" },
                    { 104, true, 1, "Videojuegos y Entornos 3D" },
                    { 201, true, 2, "Sistemas Operativos y Redes" },
                    { 202, true, 2, "Cloud Computing" },
                    { 203, true, 2, "Ciberseguridad y Hacking Ético" },
                    { 204, true, 2, "DevOps y Automatización" },
                    { 301, true, 3, "Inteligencia Artificial Aplicada" },
                    { 302, true, 3, "Ciencia de Datos y Big Data" },
                    { 303, true, 3, "Business Intelligence (BI)" },
                    { 401, true, 4, "Diseño UX/UI y Prototipado" },
                    { 402, true, 4, "Metodologías Ágiles (Agile)" },
                    { 403, true, 4, "Marketing Digital y Growth" },
                    { 501, true, 5, "Orientación Laboral y Marca Personal" },
                    { 502, true, 5, "Habilidades Blandas (Soft Skills)" },
                    { 503, true, 5, "Emprendimiento y Startups" }
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
                name: "IX_Centers_Email",
                table: "Centers",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Centers_IsActive",
                table: "Centers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Centers_Locality",
                table: "Centers",
                column: "Locality");

            migrationBuilder.CreateIndex(
                name: "IX_Events_IsActive",
                table: "Events",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartDate",
                table: "Events",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StartDate_EndDate",
                table: "Events",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_FPTours_AmbassadorId",
                table: "FPTours",
                column: "AmbassadorId");

            migrationBuilder.CreateIndex(
                name: "IX_FPTours_CenterId",
                table: "FPTours",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_FPTours_HasScheduledDate",
                table: "FPTours",
                column: "HasScheduledDate");

            migrationBuilder.CreateIndex(
                name: "IX_FPTours_IsActive",
                table: "FPTours",
                column: "IsActive");

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

            migrationBuilder.CreateIndex(
                name: "IX_MT_Categories_Active",
                table: "MT_Categories",
                column: "Active");

            migrationBuilder.CreateIndex(
                name: "IX_MT_Categories_FatherId",
                table: "MT_Categories",
                column: "FatherId");

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
                name: "IX_Sessions_EventId",
                table: "Sessions",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_EventId_StartTime",
                table: "Sessions",
                columns: new[] { "EventId", "StartTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_IsActive",
                table: "Sessions",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Speaker",
                table: "Sessions",
                column: "Speaker");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "candidaturas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "FPTours");

            migrationBuilder.DropTable(
                name: "intranet_audit_logs",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "intranet_settings",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "intranet_user_categories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "tutoriales",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ofertas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Ambassadors");

            migrationBuilder.DropTable(
                name: "Centers");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "MT_Categories");
        }
    }
}
