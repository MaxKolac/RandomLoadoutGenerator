using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RandomLoadoutGenerator.Migrations
{
    /// <inheritdoc />
    internal partial class InitialScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoadoutCombinations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Slot = table.Column<int>(type: "INTEGER", nullable: false),
                    Class = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadoutCombinations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReskinGroups",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReskinGroups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ImageURI = table.Column<string>(type: "TEXT", nullable: false),
                    IsStock = table.Column<bool>(type: "INTEGER", nullable: false),
                    ReskinGroupID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Weapons_ReskinGroups_ReskinGroupID",
                        column: x => x.ReskinGroupID,
                        principalTable: "ReskinGroups",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "LoadoutCombinationWeapon",
                columns: table => new
                {
                    LoadoutCombosID = table.Column<int>(type: "INTEGER", nullable: false),
                    WeaponsID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadoutCombinationWeapon", x => new { x.LoadoutCombosID, x.WeaponsID });
                    table.ForeignKey(
                        name: "FK_LoadoutCombinationWeapon_LoadoutCombinations_LoadoutCombosID",
                        column: x => x.LoadoutCombosID,
                        principalTable: "LoadoutCombinations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoadoutCombinationWeapon_Weapons_WeaponsID",
                        column: x => x.WeaponsID,
                        principalTable: "Weapons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadoutCombinationWeapon_WeaponsID",
                table: "LoadoutCombinationWeapon",
                column: "WeaponsID");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_ReskinGroupID",
                table: "Weapons",
                column: "ReskinGroupID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadoutCombinationWeapon");

            migrationBuilder.DropTable(
                name: "LoadoutCombinations");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "ReskinGroups");
        }
    }
}
