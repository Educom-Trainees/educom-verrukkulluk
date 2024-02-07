using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verrukkulluk.Migrations
{
    /// <inheritdoc />
    public partial class AddedFkBetweenProductAndImageObj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoLocation",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "ImageObjId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ImageObjId",
                table: "Products",
                column: "ImageObjId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ImageObjs_ImageObjId",
                table: "Products",
                column: "ImageObjId",
                principalTable: "ImageObjs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ImageObjs_ImageObjId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ImageObjId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageObjId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "PhotoLocation",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
