using Microsoft.EntityFrameworkCore.Migrations;

namespace ResturantSearchServiceApi.Migrations
{
    public partial class m1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "MenuCategory_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "MenuItem_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "MenuType_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "MenuCategory",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    Category = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MenuType",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    FoodType = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MenuItem",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    MenuTypeID = table.Column<int>(nullable: false),
                    MenuCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_MenuItem_MenuCategory_MenuCategoryId",
                        column: x => x.MenuCategoryId,
                        principalTable: "MenuCategory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItem_MenuType_MenuTypeID",
                        column: x => x.MenuTypeID,
                        principalTable: "MenuType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_MenuCategoryId",
                table: "MenuItem",
                column: "MenuCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_MenuTypeID",
                table: "MenuItem",
                column: "MenuTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItem");

            migrationBuilder.DropTable(
                name: "MenuCategory");

            migrationBuilder.DropTable(
                name: "MenuType");

            migrationBuilder.DropSequence(
                name: "MenuCategory_hilo");

            migrationBuilder.DropSequence(
                name: "MenuItem_hilo");

            migrationBuilder.DropSequence(
                name: "MenuType_hilo");
        }
    }
}
