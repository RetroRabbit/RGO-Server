using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class removeevaluations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeEvaluationAudience");

            migrationBuilder.DropTable(
                name: "EmployeeEvaluationRatings");

            migrationBuilder.DropTable(
                name: "EmployeeEvaluationTemplateItem");

            migrationBuilder.DropTable(
                name: "EmployeeEvaluations");

            migrationBuilder.DropTable(
                name: "EmployeeEvaluationTemplate");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeRole_employeeId",
                table: "EmployeeRole");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRole_employeeId",
                table: "EmployeeRole",
                column: "employeeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmployeeRole_employeeId",
                table: "EmployeeRole");

            migrationBuilder.CreateTable(
                name: "EmployeeEvaluationTemplate",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvaluationTemplate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEvaluations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    ownerId = table.Column<int>(type: "integer", nullable: false),
                    templateId = table.Column<int>(type: "integer", nullable: false),
                    endDate = table.Column<DateOnly>(type: "date", nullable: true),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvaluations", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluations_EmployeeEvaluationTemplate_templateId",
                        column: x => x.templateId,
                        principalTable: "EmployeeEvaluationTemplate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluations_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluations_Employee_ownerId",
                        column: x => x.ownerId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEvaluationTemplateItem",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    templateId = table.Column<int>(type: "integer", nullable: false),
                    question = table.Column<string>(type: "text", nullable: false),
                    section = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvaluationTemplateItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluationTemplateItem_EmployeeEvaluationTemplate_t~",
                        column: x => x.templateId,
                        principalTable: "EmployeeEvaluationTemplate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEvaluationAudience",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeEvaluationId = table.Column<int>(type: "integer", nullable: false),
                    employeeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvaluationAudience", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluationAudience_EmployeeEvaluations_employeeEval~",
                        column: x => x.employeeEvaluationId,
                        principalTable: "EmployeeEvaluations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluationAudience_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEvaluationRatings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeEvaluationId = table.Column<int>(type: "integer", nullable: false),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    score = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvaluationRatings", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluationRatings_EmployeeEvaluations_employeeEvalu~",
                        column: x => x.employeeEvaluationId,
                        principalTable: "EmployeeEvaluations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluationRatings_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRole_employeeId",
                table: "EmployeeRole",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluationAudience_employeeEvaluationId",
                table: "EmployeeEvaluationAudience",
                column: "employeeEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluationAudience_employeeId",
                table: "EmployeeEvaluationAudience",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluationRatings_employeeEvaluationId",
                table: "EmployeeEvaluationRatings",
                column: "employeeEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluationRatings_employeeId",
                table: "EmployeeEvaluationRatings",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluations_employeeId",
                table: "EmployeeEvaluations",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluations_ownerId",
                table: "EmployeeEvaluations",
                column: "ownerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluations_templateId",
                table: "EmployeeEvaluations",
                column: "templateId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluationTemplateItem_templateId",
                table: "EmployeeEvaluationTemplateItem",
                column: "templateId");
        }
    }
}
