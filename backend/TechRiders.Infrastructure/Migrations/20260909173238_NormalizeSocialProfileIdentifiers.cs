using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechRiders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeSocialProfileIdentifiers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discord",
                table: "CommunityPartnerApplications");

            migrationBuilder.DropColumn(
                name: "Telegram",
                table: "CommunityPartnerApplications");

            migrationBuilder.AddColumn<string>(
                name: "X",
                table: "Users",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YouTube",
                table: "Users",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "YouTube",
                table: "CommunityPartnerApplications",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "X",
                table: "CommunityPartnerApplications",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkedIn",
                table: "CommunityPartnerApplications",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Instagram",
                table: "CommunityPartnerApplications",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Github",
                table: "CommunityPartnerApplications",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Github",
                table: "Communities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "X",
                table: "Communities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YouTube",
                table: "Communities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Github",
                table: "Centers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "X",
                table: "Centers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YouTube",
                table: "Centers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE Users
                SET LinkedIn = CASE
                    WHEN NULLIF(LTRIM(RTRIM(LinkedIn)), '') IS NULL THEN NULL
                    WHEN LinkedIn LIKE 'https://www.linkedin.com/in/%' THEN SUBSTRING(LinkedIn, LEN('https://www.linkedin.com/in/') + 1, 300)
                    WHEN LinkedIn LIKE 'https://linkedin.com/in/%' THEN SUBSTRING(LinkedIn, LEN('https://linkedin.com/in/') + 1, 300)
                    WHEN LinkedIn NOT LIKE '%://%' AND LinkedIn NOT LIKE '%/%' THEN LTRIM(RTRIM(LinkedIn))
                    ELSE NULL
                END,
                    Instagram = CASE
                    WHEN NULLIF(LTRIM(RTRIM(Instagram)), '') IS NULL THEN NULL
                    WHEN Instagram LIKE 'https://www.instagram.com/%' THEN SUBSTRING(Instagram, LEN('https://www.instagram.com/') + 1, 300)
                    WHEN Instagram LIKE 'https://instagram.com/%' THEN SUBSTRING(Instagram, LEN('https://instagram.com/') + 1, 300)
                    WHEN Instagram NOT LIKE '%://%' AND Instagram NOT LIKE '%/%' THEN LTRIM(RTRIM(Instagram))
                    ELSE NULL
                END,
                    Github = CASE
                    WHEN NULLIF(LTRIM(RTRIM(Github)), '') IS NULL THEN NULL
                    WHEN Github LIKE 'https://github.com/%' THEN SUBSTRING(Github, LEN('https://github.com/') + 1, 300)
                    WHEN Github NOT LIKE '%://%' AND Github NOT LIKE '%/%' THEN LTRIM(RTRIM(Github))
                    ELSE NULL
                END;

                UPDATE Centers
                SET LinkedIn = CASE
                    WHEN NULLIF(LTRIM(RTRIM(LinkedIn)), '') IS NULL THEN NULL
                    WHEN LinkedIn LIKE 'https://www.linkedin.com/%' THEN SUBSTRING(LinkedIn, LEN('https://www.linkedin.com/') + 1, 300)
                    WHEN LinkedIn NOT LIKE '%://%' AND LinkedIn NOT LIKE '%/%' THEN LTRIM(RTRIM(LinkedIn))
                    ELSE NULL
                END,
                    Instagram = CASE
                    WHEN NULLIF(LTRIM(RTRIM(Instagram)), '') IS NULL THEN NULL
                    WHEN Instagram LIKE 'https://www.instagram.com/%' THEN SUBSTRING(Instagram, LEN('https://www.instagram.com/') + 1, 300)
                    WHEN Instagram LIKE 'https://instagram.com/%' THEN SUBSTRING(Instagram, LEN('https://instagram.com/') + 1, 300)
                    WHEN Instagram NOT LIKE '%://%' AND Instagram NOT LIKE '%/%' THEN LTRIM(RTRIM(Instagram))
                    ELSE NULL
                END;

                UPDATE Communities
                SET LinkedIn = CASE
                    WHEN NULLIF(LTRIM(RTRIM(LinkedIn)), '') IS NULL THEN NULL
                    WHEN LinkedIn LIKE 'https://www.linkedin.com/%' THEN SUBSTRING(LinkedIn, LEN('https://www.linkedin.com/') + 1, 300)
                    WHEN LinkedIn NOT LIKE '%://%' AND LinkedIn NOT LIKE '%/%' THEN LTRIM(RTRIM(LinkedIn))
                    ELSE NULL
                END,
                    Instagram = CASE
                    WHEN NULLIF(LTRIM(RTRIM(Instagram)), '') IS NULL THEN NULL
                    WHEN Instagram LIKE 'https://www.instagram.com/%' THEN SUBSTRING(Instagram, LEN('https://www.instagram.com/') + 1, 300)
                    WHEN Instagram LIKE 'https://instagram.com/%' THEN SUBSTRING(Instagram, LEN('https://instagram.com/') + 1, 300)
                    WHEN Instagram NOT LIKE '%://%' AND Instagram NOT LIKE '%/%' THEN LTRIM(RTRIM(Instagram))
                    ELSE NULL
                END;

                UPDATE CommunityPartnerApplications
                SET LinkedIn = CASE
                    WHEN NULLIF(LTRIM(RTRIM(LinkedIn)), '') IS NULL THEN NULL
                    WHEN LinkedIn LIKE 'https://www.linkedin.com/%' THEN SUBSTRING(LinkedIn, LEN('https://www.linkedin.com/') + 1, 300)
                    WHEN LinkedIn NOT LIKE '%://%' AND LinkedIn NOT LIKE '%/%' THEN LTRIM(RTRIM(LinkedIn))
                    ELSE NULL
                END,
                    Instagram = CASE
                    WHEN NULLIF(LTRIM(RTRIM(Instagram)), '') IS NULL THEN NULL
                    WHEN Instagram LIKE 'https://www.instagram.com/%' THEN SUBSTRING(Instagram, LEN('https://www.instagram.com/') + 1, 300)
                    WHEN Instagram LIKE 'https://instagram.com/%' THEN SUBSTRING(Instagram, LEN('https://instagram.com/') + 1, 300)
                    WHEN Instagram NOT LIKE '%://%' AND Instagram NOT LIKE '%/%' THEN LTRIM(RTRIM(Instagram))
                    ELSE NULL
                END,
                    X = CASE
                    WHEN NULLIF(LTRIM(RTRIM(X)), '') IS NULL THEN NULL
                    WHEN X LIKE 'https://x.com/%' THEN SUBSTRING(X, LEN('https://x.com/') + 1, 300)
                    WHEN X LIKE 'https://twitter.com/%' THEN SUBSTRING(X, LEN('https://twitter.com/') + 1, 300)
                    WHEN X NOT LIKE '%://%' AND X NOT LIKE '%/%' THEN LTRIM(RTRIM(X))
                    ELSE NULL
                END,
                    YouTube = CASE
                    WHEN NULLIF(LTRIM(RTRIM(YouTube)), '') IS NULL THEN NULL
                    WHEN YouTube LIKE 'https://www.youtube.com/@%' THEN SUBSTRING(YouTube, LEN('https://www.youtube.com/@') + 1, 300)
                    WHEN YouTube LIKE 'https://www.youtube.com/%' THEN SUBSTRING(YouTube, LEN('https://www.youtube.com/') + 1, 300)
                    WHEN YouTube NOT LIKE '%://%' AND YouTube NOT LIKE '%/%' THEN LTRIM(RTRIM(YouTube))
                    ELSE NULL
                END;

                UPDATE Users
                SET LinkedIn = CASE
                    WHEN LinkedIn LIKE 'in/%' THEN SUBSTRING(LinkedIn, LEN('in/') + 1, 300)
                    WHEN LinkedIn LIKE 'company/%' THEN SUBSTRING(LinkedIn, LEN('company/') + 1, 300)
                    WHEN LinkedIn LIKE 'school/%' THEN SUBSTRING(LinkedIn, LEN('school/') + 1, 300)
                    ELSE LinkedIn
                END;

                UPDATE Centers
                SET LinkedIn = CASE
                    WHEN LinkedIn LIKE 'in/%' THEN SUBSTRING(LinkedIn, LEN('in/') + 1, 300)
                    WHEN LinkedIn LIKE 'company/%' THEN SUBSTRING(LinkedIn, LEN('company/') + 1, 300)
                    WHEN LinkedIn LIKE 'school/%' THEN SUBSTRING(LinkedIn, LEN('school/') + 1, 300)
                    ELSE LinkedIn
                END;

                UPDATE Communities
                SET LinkedIn = CASE
                    WHEN LinkedIn LIKE 'in/%' THEN SUBSTRING(LinkedIn, LEN('in/') + 1, 300)
                    WHEN LinkedIn LIKE 'company/%' THEN SUBSTRING(LinkedIn, LEN('company/') + 1, 300)
                    WHEN LinkedIn LIKE 'school/%' THEN SUBSTRING(LinkedIn, LEN('school/') + 1, 300)
                    ELSE LinkedIn
                END;

                UPDATE CommunityPartnerApplications
                SET LinkedIn = CASE
                    WHEN LinkedIn LIKE 'in/%' THEN SUBSTRING(LinkedIn, LEN('in/') + 1, 300)
                    WHEN LinkedIn LIKE 'company/%' THEN SUBSTRING(LinkedIn, LEN('company/') + 1, 300)
                    WHEN LinkedIn LIKE 'school/%' THEN SUBSTRING(LinkedIn, LEN('school/') + 1, 300)
                    ELSE LinkedIn
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "YouTube",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Github",
                table: "CommunityPartnerApplications");

            migrationBuilder.DropColumn(
                name: "Github",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "X",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "YouTube",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "Github",
                table: "Centers");

            migrationBuilder.DropColumn(
                name: "X",
                table: "Centers");

            migrationBuilder.DropColumn(
                name: "YouTube",
                table: "Centers");

            migrationBuilder.AlterColumn<string>(
                name: "YouTube",
                table: "CommunityPartnerApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "X",
                table: "CommunityPartnerApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LinkedIn",
                table: "CommunityPartnerApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Instagram",
                table: "CommunityPartnerApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discord",
                table: "CommunityPartnerApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telegram",
                table: "CommunityPartnerApplications",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
