using Microsoft.EntityFrameworkCore.Migrations;

namespace ExpenseApp.Migrations
{
    public partial class some : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Locations_LocationName",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "PaymentReceiptNumber",
                table: "ExpenseForms",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LocationName",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Locations_LocationName",
                table: "Employees",
                column: "LocationName",
                principalTable: "Locations",
                principalColumn: "Name",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Locations_LocationName",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PaymentReceiptNumber",
                table: "ExpenseForms");

            migrationBuilder.AlterColumn<string>(
                name: "LocationName",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Locations_LocationName",
                table: "Employees",
                column: "LocationName",
                principalTable: "Locations",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
