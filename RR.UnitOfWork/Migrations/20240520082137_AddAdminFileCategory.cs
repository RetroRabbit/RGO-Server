using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminFileCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "adminFileCategory",
                table: "EmployeeDocument",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adminFileCategory",
                table: "EmployeeDocument");
        }
    }
}
