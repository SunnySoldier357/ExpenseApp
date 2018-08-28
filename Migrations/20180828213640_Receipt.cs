using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseApp.Migrations
{
    public partial class Receipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ReceiptId",
                table: "ExpenseEntries",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseEntries_ReceiptId",
                table: "ExpenseEntries",
                column: "ReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseEntries_Receipts_ReceiptId",
                table: "ExpenseEntries",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseEntries_Receipts_ReceiptId",
                table: "ExpenseEntries");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseEntries_ReceiptId",
                table: "ExpenseEntries");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiptId",
                table: "ExpenseEntries",
                nullable: true,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}