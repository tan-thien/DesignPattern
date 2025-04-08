using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStore.Migrations
{
    /// <inheritdoc />
    public partial class AddIdCateToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryIdCate",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CategoryIdCate",
                table: "Products",
                newName: "IdCate");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryIdCate",
                table: "Products",
                newName: "IX_Products_IdCate");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_IdCate",
                table: "Products",
                column: "IdCate",
                principalTable: "Categories",
                principalColumn: "IdCate",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_IdCate",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "IdCate",
                table: "Products",
                newName: "CategoryIdCate");

            migrationBuilder.RenameIndex(
                name: "IX_Products_IdCate",
                table: "Products",
                newName: "IX_Products_CategoryIdCate");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryIdCate",
                table: "Products",
                column: "CategoryIdCate",
                principalTable: "Categories",
                principalColumn: "IdCate",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
