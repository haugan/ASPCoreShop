using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreShop.Data.Migrations
{
    public partial class CustomerCartItemsLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerID",
                table: "CartItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CustomerID",
                table: "CartItems",
                column: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Customers_CustomerID",
                table: "CartItems",
                column: "CustomerID",
                principalTable: "Customers",
                principalColumn: "CustomerID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Customers_CustomerID",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CustomerID",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CustomerID",
                table: "CartItems");
        }
    }
}
