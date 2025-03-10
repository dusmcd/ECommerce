using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class ProductOrderAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOrders_Orders_OrdersId",
                table: "ProductsOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOrders_Products_ProductsId",
                table: "ProductsOrders");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "ProductsOrders",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "OrdersId",
                table: "ProductsOrders",
                newName: "OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsOrders_ProductsId",
                table: "ProductsOrders",
                newName: "IX_ProductsOrders_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsOrders_OrdersId",
                table: "ProductsOrders",
                newName: "IX_ProductsOrders_OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOrders_Orders_OrderId",
                table: "ProductsOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOrders_Products_ProductId",
                table: "ProductsOrders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOrders_Orders_OrderId",
                table: "ProductsOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOrders_Products_ProductId",
                table: "ProductsOrders");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ProductsOrders",
                newName: "ProductsId");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "ProductsOrders",
                newName: "OrdersId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsOrders_ProductId",
                table: "ProductsOrders",
                newName: "IX_ProductsOrders_ProductsId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsOrders_OrderId",
                table: "ProductsOrders",
                newName: "IX_ProductsOrders_OrdersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOrders_Orders_OrdersId",
                table: "ProductsOrders",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOrders_Products_ProductsId",
                table: "ProductsOrders",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
