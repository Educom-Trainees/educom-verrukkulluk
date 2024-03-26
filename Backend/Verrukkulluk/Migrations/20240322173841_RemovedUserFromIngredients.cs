﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Verrukkulluk.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUserFromIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_AspNetUsers_UserId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_UserId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Ingredients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Ingredients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_UserId",
                table: "Ingredients",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_AspNetUsers_UserId",
                table: "Ingredients",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
