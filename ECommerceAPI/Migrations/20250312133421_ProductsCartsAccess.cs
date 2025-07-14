using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProductsCartsAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsCarts_Carts_CartsId",
                table: "ProductsCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsCarts_Products_ProductsId",
                table: "ProductsCarts");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "ProductsCarts",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "CartsId",
                table: "ProductsCarts",
                newName: "CartId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsCarts_ProductsId",
                table: "ProductsCarts",
                newName: "IX_ProductsCarts_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsCarts_CartsId",
                table: "ProductsCarts",
                newName: "IX_ProductsCarts_CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsCarts_Carts_CartId",
                table: "ProductsCarts",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsCarts_Products_ProductId",
                table: "ProductsCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsCarts_Carts_CartId",
                table: "ProductsCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsCarts_Products_ProductId",
                table: "ProductsCarts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductsCarts",
                newName: "ProductsId");

            migrationBuilder.RenameColumn(
                name: "CartId",
                table: "ProductsCarts",
                newName: "CartsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsCarts_ProductId",
                table: "ProductsCarts",
                newName: "IX_ProductsCarts_ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsCarts_CartId",
                table: "ProductsCarts",
                newName: "IX_ProductsCarts_CartsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsCarts_Carts_CartsId",
                table: "ProductsCarts",
                column: "CartsId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsCarts_Products_ProductsId",
                table: "ProductsCarts",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
