using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class updatedWorkExperienceCRUD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "companyName",
                table: "WorkExperience");

            migrationBuilder.DropColumn(
                name: "title",
                table: "WorkExperience");

            migrationBuilder.RenameColumn(
                name: "location",
                table: "WorkExperience",
                newName: "projectName");

            migrationBuilder.RenameColumn(
                name: "employmentType",
                table: "WorkExperience",
                newName: "clientName");

            migrationBuilder.AddColumn<List<string>>(
                name: "skillSet",
                table: "WorkExperience",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "software",
                table: "WorkExperience",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "skillSet",
                table: "WorkExperience");

            migrationBuilder.DropColumn(
                name: "software",
                table: "WorkExperience");

            migrationBuilder.RenameColumn(
                name: "projectName",
                table: "WorkExperience",
                newName: "location");

            migrationBuilder.RenameColumn(
                name: "clientName",
                table: "WorkExperience",
                newName: "employmentType");

            migrationBuilder.AddColumn<string>(
                name: "companyName",
                table: "WorkExperience",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "WorkExperience",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
