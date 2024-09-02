using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class movingTaxNumbr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "taxNumber",
                table: "Employee");

            migrationBuilder.AddColumn<string>(
                name: "taxNumber",
                table: "EmployeeSalaryDetails",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "taxNumber",
                table: "EmployeeSalaryDetails");

            migrationBuilder.AddColumn<string>(
                name: "taxNumber",
                table: "Employee",
                type: "text",
                nullable: true);
        }
    }
}
