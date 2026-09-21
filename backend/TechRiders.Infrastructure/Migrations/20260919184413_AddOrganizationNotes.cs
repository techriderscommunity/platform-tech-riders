using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechRiders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Organizations");
        }
    }
}
