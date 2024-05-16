using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class employeeQualificationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeQualifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    highestQualification = table.Column<int>(type: "integer", nullable: false),
                    school = table.Column<string>(type: "text", nullable: true),
                    degree = table.Column<string>(type: "text", nullable: true),
                    fieldOfStudy = table.Column<string>(type: "text", nullable: true),
                    nqfLevel = table.Column<int>(type: "integer", nullable: false),
                    year = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeQualifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeQualifications_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeQualifications_employeeId",
                table: "EmployeeQualifications",
                column: "employeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeQualifications");
        }
    }
}
