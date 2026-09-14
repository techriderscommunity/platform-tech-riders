using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechRiders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLegacyCompanyCenterModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                        migrationBuilder.Sql("""
                                INSERT INTO Organizations (Id, OrganizationType, Name, TaxId, Website, Address, Province, Origin, CreatedAt, IsActive)
                                SELECT c.Id, 1, c.Name, NULL, NULL, c.Location, c.Locality, CONCAT('legacy-center:', CONVERT(nvarchar(36), c.Id)), c.CreatedAt, c.IsActive
                                FROM Centers c
                                WHERE NOT EXISTS (SELECT 1 FROM Organizations o WHERE o.Id = c.Id OR o.Origin = CONCAT('legacy-center:', CONVERT(nvarchar(36), c.Id)));

                                INSERT INTO Organizations (Id, OrganizationType, Name, TaxId, Website, Address, Province, Origin, CreatedAt, IsActive)
                                SELECT c.Id, 3, c.Name, NULL, c.Website, NULL, NULL, CONCAT('legacy-company:', CONVERT(nvarchar(36), c.Id)), c.CreatedAt, c.IsActive
                                FROM Companies c
                                WHERE NOT EXISTS (SELECT 1 FROM Organizations o WHERE o.Id = c.Id OR o.Origin = CONCAT('legacy-company:', CONVERT(nvarchar(36), c.Id)));

                                INSERT INTO PersonOrganizations (Id, UserId, OrganizationId, Position, RelationType, IsPrimaryContact, Status, StartDate, EndDate, ValidatedByUserId, ValidatedAt, CreatedAt, IsActive)
                                SELECT NEWID(), cc.UserId, cc.CenterId, cc.Role, 7, 1, 2, cc.CreatedAt, NULL, NULL, cc.CreatedAt, cc.CreatedAt, cc.IsActive
                                FROM CenterContacts cc
                                WHERE cc.UserId IS NOT NULL
                                    AND NOT EXISTS (SELECT 1 FROM PersonOrganizations po WHERE po.UserId = cc.UserId AND po.OrganizationId = cc.CenterId);

                                INSERT INTO PersonOrganizations (Id, UserId, OrganizationId, Position, RelationType, IsPrimaryContact, Status, StartDate, EndDate, ValidatedByUserId, ValidatedAt, CreatedAt, IsActive)
                                SELECT NEWID(), c.ContactUserId, c.Id, 'Contacto legacy', 7, 1, 2, c.CreatedAt, NULL, NULL, c.CreatedAt, c.CreatedAt, c.IsActive
                                FROM Companies c
                                WHERE c.ContactUserId IS NOT NULL
                                    AND NOT EXISTS (SELECT 1 FROM PersonOrganizations po WHERE po.UserId = c.ContactUserId AND po.OrganizationId = c.Id);

                                INSERT INTO GpfPersonLinks (Id, UserId, CodUnico, Status, LinkedAt, LastQueriedAt, LinkMethod, ValidatedByUserId, CreatedAt, IsActive)
                                SELECT NEWID(), u.Id, u.GPFId, 1, u.CreatedAt, NULL, 'legacy-user-gpfid', NULL, u.CreatedAt, u.IsActive
                                FROM Users u
                                WHERE u.GPFId IS NOT NULL
                                    AND NOT EXISTS (SELECT 1 FROM GpfPersonLinks l WHERE l.UserId = u.Id OR l.CodUnico = u.GPFId);
                                """);

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Centers_CenterId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_FPTours_Centers_CenterId",
                table: "FPTours");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Centers_CenterId",
                table: "Sessions");

            migrationBuilder.DropTable(
                name: "CenterContacts");

            migrationBuilder.DropTable(
                name: "CenterStudies");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Centers");

            migrationBuilder.DropColumn(
                name: "GPFId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CenterId",
                table: "Sessions",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_CenterId",
                table: "Sessions",
                newName: "IX_Sessions_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "CenterId",
                table: "FPTours",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_FPTours_CenterId",
                table: "FPTours",
                newName: "IX_FPTours_OrganizationId");

            migrationBuilder.RenameColumn(
                name: "CenterId",
                table: "Events",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_CenterId",
                table: "Events",
                newName: "IX_Events_OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Organizations_OrganizationId",
                table: "Events",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FPTours_Organizations_OrganizationId",
                table: "FPTours",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Organizations_OrganizationId",
                table: "Sessions",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Organizations_OrganizationId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_FPTours_Organizations_OrganizationId",
                table: "FPTours");

            migrationBuilder.DropForeignKey(
                name: "FK_Sessions_Organizations_OrganizationId",
                table: "Sessions");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Sessions",
                newName: "CenterId");

            migrationBuilder.RenameIndex(
                name: "IX_Sessions_OrganizationId",
                table: "Sessions",
                newName: "IX_Sessions_CenterId");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "FPTours",
                newName: "CenterId");

            migrationBuilder.RenameIndex(
                name: "IX_FPTours_OrganizationId",
                table: "FPTours",
                newName: "IX_FPTours_CenterId");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "Events",
                newName: "CenterId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_OrganizationId",
                table: "Events",
                newName: "IX_Events_CenterId");

            migrationBuilder.AddColumn<string>(
                name: "GPFId",
                table: "Users",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Centers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Github = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Instagram = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Locality = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Parking = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParkingInfo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Specialty = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    X = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    YouTube = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Centers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LinkedIn = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(180)", maxLength: 180, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
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
                name: "CenterContacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "CenterStudies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_CenterContacts_CenterId",
                table: "CenterContacts",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_CenterContacts_UserId",
                table: "CenterContacts",
                column: "UserId");

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
                name: "IX_CenterStudies_CenterId",
                table: "CenterStudies",
                column: "CenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ContactUserId",
                table: "Companies",
                column: "ContactUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Centers_CenterId",
                table: "Events",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_FPTours_Centers_CenterId",
                table: "FPTours",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Sessions_Centers_CenterId",
                table: "Sessions",
                column: "CenterId",
                principalTable: "Centers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
