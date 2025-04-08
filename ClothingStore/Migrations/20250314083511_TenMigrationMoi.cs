using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothingStore.Migrations
{
    /// <inheritdoc />
    public partial class TenMigrationMoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Accounts_AccountIdAcc",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_AccountIdAcc",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AccountIdAcc",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdAcc",
                table: "Users",
                column: "IdAcc");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Accounts_IdAcc",
                table: "Users",
                column: "IdAcc",
                principalTable: "Accounts",
                principalColumn: "IdAcc",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Accounts_IdAcc",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdAcc",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountIdAcc",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AccountIdAcc",
                table: "Users",
                column: "AccountIdAcc");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Accounts_AccountIdAcc",
                table: "Users",
                column: "AccountIdAcc",
                principalTable: "Accounts",
                principalColumn: "IdAcc",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
