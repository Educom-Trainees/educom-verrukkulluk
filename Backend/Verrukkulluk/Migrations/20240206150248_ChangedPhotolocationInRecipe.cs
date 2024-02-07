using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verrukkulluk.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPhotolocationInRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DishPhoto",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "PhotoLocation",
                table: "Recipes");

            migrationBuilder.AddColumn<int>(
                name: "ImageObjId",
                table: "Recipes",
                type: "int",
                nullable: false,
                defaultValue: 0);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageObjId",
                table: "Recipes");

            migrationBuilder.AddColumn<byte[]>(
                name: "DishPhoto",
                table: "Recipes",
                type: "longblob",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "PhotoLocation",
                table: "Recipes",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
