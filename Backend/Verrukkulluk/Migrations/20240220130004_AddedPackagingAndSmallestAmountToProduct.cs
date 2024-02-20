using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verrukkulluk.Migrations
{
    /// <inheritdoc />
    public partial class AddedPackagingAndSmallestAmountToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackagingId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SmallestAmount",
                table: "Products",
                type: "double",
                nullable: false,
                defaultValue: 1.0);

            migrationBuilder.CreateTable(
                name: "PackagingTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackagingTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Products_PackagingId",
                table: "Products",
                column: "PackagingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PackagingTypes_PackagingId",
                table: "Products",
                column: "PackagingId",
                principalTable: "PackagingTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PackagingTypes_PackagingId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "PackagingTypes");

            migrationBuilder.DropIndex(
                name: "IX_Products_PackagingId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PackagingId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SmallestAmount",
                table: "Products");
        }
    }
}
