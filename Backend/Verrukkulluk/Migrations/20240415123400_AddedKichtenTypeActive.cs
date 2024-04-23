using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verrukkulluk.Migrations
{
    /// <inheritdoc />
    public partial class AddedKichtenTypeActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "KitchenTypes",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "KitchenTypes");
        }
    }
}
