using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseApp.Migrations
{
    public partial class Reverse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseEntries_ExpenseForms_ExpenseFormStatementNumber",
                table: "ExpenseEntries");

            migrationBuilder.RenameColumn(
                name: "ExpenseFormStatementNumber",
                table: "ExpenseEntries",
                newName: "FormStatementNumber");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseEntries_ExpenseFormStatementNumber",
                table: "ExpenseEntries",
                newName: "IX_ExpenseEntries_FormStatementNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseEntries_ExpenseForms_FormStatementNumber",
                table: "ExpenseEntries",
                column: "FormStatementNumber",
                principalTable: "ExpenseForms",
                principalColumn: "StatementNumber",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseEntries_ExpenseForms_FormStatementNumber",
                table: "ExpenseEntries");

            migrationBuilder.RenameColumn(
                name: "FormStatementNumber",
                table: "ExpenseEntries",
                newName: "ExpenseFormStatementNumber");

            migrationBuilder.RenameIndex(
                name: "IX_ExpenseEntries_FormStatementNumber",
                table: "ExpenseEntries",
                newName: "IX_ExpenseEntries_ExpenseFormStatementNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseEntries_ExpenseForms_ExpenseFormStatementNumber",
                table: "ExpenseEntries",
                column: "ExpenseFormStatementNumber",
                principalTable: "ExpenseForms",
                principalColumn: "StatementNumber",
                onDelete: ReferentialAction.Restrict);
        }
    }
}