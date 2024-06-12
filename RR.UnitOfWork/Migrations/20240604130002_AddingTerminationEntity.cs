using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class AddingTerminationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "startDate",
                table: "WorkExperience",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "endDate",
                table: "WorkExperience",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.CreateTable(
                name: "Termination",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    terminationOption = table.Column<int>(type: "integer", nullable: false),
                    dayOfNotice = table.Column<DateOnly>(type: "date", nullable: false),
                    lastDayOfEmployment = table.Column<DateOnly>(type: "date", nullable: false),
                    reemploymentStatus = table.Column<bool>(type: "boolean", nullable: false),
                    equipmentStatus = table.Column<bool>(type: "boolean", nullable: false),
                    accountsStatus = table.Column<bool>(type: "boolean", nullable: false),
                    terminationDocument = table.Column<string>(type: "text", nullable: false),
                    documentName = table.Column<string>(type: "text", nullable: false),
                    terminationComments = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Termination", x => x.id);
                    table.ForeignKey(
                        name: "FK_Termination_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Termination_employeeId",
                table: "Termination",
                column: "employeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Termination");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "startDate",
                table: "WorkExperience",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "endDate",
                table: "WorkExperience",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
