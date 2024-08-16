using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class removedAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeAddress_physicalAddress",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeAddress_postalAddress",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_physicalAddress",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_postalAddress",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "physicalAddress",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "postalAddress",
                table: "Employee");

            migrationBuilder.AddColumn<int>(
                name: "employeeId",
                table: "EmployeeAddress",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "employeeId",
                table: "EmployeeAddress");

            migrationBuilder.AddColumn<int>(
                name: "physicalAddress",
                table: "Employee",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "postalAddress",
                table: "Employee",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_physicalAddress",
                table: "Employee",
                column: "physicalAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_postalAddress",
                table: "Employee",
                column: "postalAddress");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeAddress_physicalAddress",
                table: "Employee",
                column: "physicalAddress",
                principalTable: "EmployeeAddress",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeAddress_postalAddress",
                table: "Employee",
                column: "postalAddress",
                principalTable: "EmployeeAddress",
                principalColumn: "id");
        }
    }
}
