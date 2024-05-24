using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class AddingProofOfQualification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "degree",
                table: "EmployeeQualifications",
                newName: "proofOfQualification");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "proofOfQualification",
                table: "EmployeeQualifications",
                newName: "degree");
        }
    }
}
