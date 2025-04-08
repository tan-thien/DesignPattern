using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStore.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKey_Order_Customer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerIdCus",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerIdCus",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerIdCus",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_IdCus",
                table: "Orders",
                column: "IdCus");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_IdCus",
                table: "Orders",
                column: "IdCus",
                principalTable: "Customers",
                principalColumn: "IdCus",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_IdCus",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_IdCus",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "CustomerIdCus",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerIdCus",
                table: "Orders",
                column: "CustomerIdCus");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerIdCus",
                table: "Orders",
                column: "CustomerIdCus",
                principalTable: "Customers",
                principalColumn: "IdCus",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
