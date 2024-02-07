using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verrukkulluk.Migrations
{
    /// <inheritdoc />
    public partial class AddedFKBetweenRecipeAndImageObj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ImageObjId",
                table: "Recipes",
                column: "ImageObjId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_ImageObjs_ImageObjId",
                table: "Recipes",
                column: "ImageObjId",
                principalTable: "ImageObjs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_ImageObjs_ImageObjId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_ImageObjId",
                table: "Recipes");
        }
    }
}
