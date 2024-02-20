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
                name: "Packaging",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Packaging",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SmallestAmount",
                table: "Products");
        }
    }
}
