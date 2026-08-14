using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBAStatisticsProject.Migrations
{
    /// <inheritdoc />
    public partial class SplitPlayerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "");
            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "");
            migrationBuilder.Sql(@"
                UPDATE ""Players""
                SET ""FirstName"" = split_part(""Name"", ' ', 1),
                ""LastName"" = CASE
                WHEN position(' ' IN ""Name"") > 0
                THEN substring(""Name"" FROM position(' ' IN ""Name"") + 1)
                ELSE ''
            END");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Players");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Players",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE ""Players""
                SET ""Name"" = trim(""FirstName"" || ' ' || ""LastName"")");

            migrationBuilder.DropColumn(name: "FirstName", table: "Players");
            migrationBuilder.DropColumn(name: "LastName", table: "Players");
        }
    }
}
