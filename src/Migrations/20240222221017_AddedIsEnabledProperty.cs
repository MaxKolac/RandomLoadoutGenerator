using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomLoadoutGenerator.Migrations
{
    /// <inheritdoc />
    internal partial class AddedIsEnabledProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "Weapons",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "Weapons");
        }
    }
}
