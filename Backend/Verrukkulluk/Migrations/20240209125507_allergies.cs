using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verrukkulluk.Migrations
{
    /// <inheritdoc />
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
#pragma warning disable IDE1006 // Naming Styles
    public partial class allergies : Migration
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductAllergies_AllergyId",
                table: "ProductAllergies",
                column: "AllergyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAllergies_ProductId",
                table: "ProductAllergies",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAllergies_Allergies_AllergyId",
                table: "ProductAllergies",
                column: "AllergyId",
                principalTable: "Allergies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAllergies_Products_ProductId",
                table: "ProductAllergies",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAllergies_Allergies_AllergyId",
                table: "ProductAllergies");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAllergies_Products_ProductId",
                table: "ProductAllergies");

            migrationBuilder.DropIndex(
                name: "IX_ProductAllergies_AllergyId",
                table: "ProductAllergies");

            migrationBuilder.DropIndex(
                name: "IX_ProductAllergies_ProductId",
                table: "ProductAllergies");
        }
    }
}
