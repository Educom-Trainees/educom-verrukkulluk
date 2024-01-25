using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verrukkulluk.Migrations
{
    /// <inheritdoc />
    public partial class CreatedManyToManyRelationBetweenRecipeAndDishType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishTypes_Recipes_RecipeId",
                table: "DishTypes");

            migrationBuilder.DropIndex(
                name: "IX_DishTypes_RecipeId",
                table: "DishTypes");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "DishTypes");

            migrationBuilder.CreateTable(
                name: "RecipeDishTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    DishTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeDishTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeDishTypes_DishTypes_DishTypeId",
                        column: x => x.DishTypeId,
                        principalTable: "DishTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeDishTypes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeDishTypes_DishTypeId",
                table: "RecipeDishTypes",
                column: "DishTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeDishTypes_RecipeId",
                table: "RecipeDishTypes",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeDishTypes");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId",
                table: "DishTypes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DishTypes_RecipeId",
                table: "DishTypes",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DishTypes_Recipes_RecipeId",
                table: "DishTypes",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
