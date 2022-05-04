using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class userCh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProduct_Basket_BasketsId",
                table: "BasketProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketProduct_Product_ProductsId",
                table: "BasketProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProduct",
                table: "OrderProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketProduct",
                table: "BasketProduct");

            migrationBuilder.RenameColumn(
                name: "ProductsId",
                table: "BasketProduct",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "BasketsId",
                table: "BasketProduct",
                newName: "BasketId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketProduct_ProductsId",
                table: "BasketProduct",
                newName: "IX_BasketProduct_ProductId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderProduct",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BasketProduct",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "BasketProduct",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProduct",
                table: "OrderProduct",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketProduct",
                table: "BasketProduct",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ProductId",
                table: "OrderProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketProduct_BasketId",
                table: "BasketProduct",
                column: "BasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProduct_Basket_BasketId",
                table: "BasketProduct",
                column: "BasketId",
                principalTable: "Basket",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProduct_Product_ProductId",
                table: "BasketProduct",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketProduct_Basket_BasketId",
                table: "BasketProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_BasketProduct_Product_ProductId",
                table: "BasketProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderProduct",
                table: "OrderProduct");

            migrationBuilder.DropIndex(
                name: "IX_OrderProduct_ProductId",
                table: "OrderProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasketProduct",
                table: "BasketProduct");

            migrationBuilder.DropIndex(
                name: "IX_BasketProduct_BasketId",
                table: "BasketProduct");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "User");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BasketProduct");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "BasketProduct");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "BasketProduct",
                newName: "ProductsId");

            migrationBuilder.RenameColumn(
                name: "BasketId",
                table: "BasketProduct",
                newName: "BasketsId");

            migrationBuilder.RenameIndex(
                name: "IX_BasketProduct_ProductId",
                table: "BasketProduct",
                newName: "IX_BasketProduct_ProductsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderProduct",
                table: "OrderProduct",
                columns: new[] { "ProductId", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasketProduct",
                table: "BasketProduct",
                columns: new[] { "BasketsId", "ProductsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProduct_Basket_BasketsId",
                table: "BasketProduct",
                column: "BasketsId",
                principalTable: "Basket",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BasketProduct_Product_ProductsId",
                table: "BasketProduct",
                column: "ProductsId",
                principalTable: "Product",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
