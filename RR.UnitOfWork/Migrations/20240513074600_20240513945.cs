using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class _20240513945 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeType_employeeTypeId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "accountHolderName",
                table: "EmployeeBanking");

            migrationBuilder.AlterColumn<int>(
                name: "employeeTypeId",
                table: "Employee",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeType_employeeTypeId",
                table: "Employee",
                column: "employeeTypeId",
                principalTable: "EmployeeType",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeType_employeeTypeId",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "accountHolderName",
                table: "EmployeeBanking",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "employeeTypeId",
                table: "Employee",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeType_employeeTypeId",
                table: "Employee",
                column: "employeeTypeId",
                principalTable: "EmployeeType",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
