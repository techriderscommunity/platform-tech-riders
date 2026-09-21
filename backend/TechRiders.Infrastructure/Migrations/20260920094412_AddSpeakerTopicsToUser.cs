using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechRiders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSpeakerTopicsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SpeakerTopics",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedAttendees",
                table: "Sessions",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpeakerTopics",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExpectedAttendees",
                table: "Sessions");
        }
    }
}
