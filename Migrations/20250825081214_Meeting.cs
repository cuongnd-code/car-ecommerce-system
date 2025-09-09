using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ManageCars.Migrations
{
    /// <inheritdoc />
    public partial class Meeting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "deposit",
                table: "Cars",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deposit",
                table: "Cars");
        }
    }
}
