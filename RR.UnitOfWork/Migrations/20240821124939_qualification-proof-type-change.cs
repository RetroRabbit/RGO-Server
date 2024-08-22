using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class qualificationprooftypechange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the old column
            migrationBuilder.DropColumn(
                name: "proofOfQualification",
                table: "EmployeeQualifications");

            // Add the new column with the updated type
            migrationBuilder.AddColumn<byte[]>(
                name: "proofOfQualification",
                table: "EmployeeQualifications",
                type: "bytea",
                nullable: false);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the bytea column
            migrationBuilder.DropColumn(
                name: "proofOfQualification",
                table: "EmployeeQualifications");

            // Add the old column back with the original type
            migrationBuilder.AddColumn<string>(
                name: "proofOfQualification",
                table: "EmployeeQualifications",
                type: "text",
                nullable: false);
        }
    }
}
