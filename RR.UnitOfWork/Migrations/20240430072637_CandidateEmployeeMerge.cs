using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class CandidateEmployeeMerge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.AddColumn<string>(
                name: "blackListReason",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "blackListStatus",
                table: "Employee",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cv",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "highestQualification",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isCandidate",
                table: "Employee",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "linkedIn",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "portfolioLink",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "portfolioPdf",
                table: "Employee",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "qualificationEndDate",
                table: "Employee",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "referal",
                table: "Employee",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "school",
                table: "Employee",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "blackListReason",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "blackListStatus",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "cv",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "highestQualification",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "isCandidate",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "linkedIn",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "portfolioLink",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "portfolioPdf",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "qualificationEndDate",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "referal",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "school",
                table: "Employee");

            migrationBuilder.CreateTable(
                name: "Candidate",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    blacklistedReason = table.Column<string>(type: "text", nullable: true),
                    blacklisted = table.Column<int>(type: "integer", nullable: false),
                    cv = table.Column<string>(type: "text", nullable: true),
                    cellphone = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    highestQualification = table.Column<string>(type: "text", nullable: true),
                    idNumber = table.Column<string>(type: "text", nullable: true),
                    jobPosition = table.Column<int>(type: "integer", nullable: false),
                    linkedIn = table.Column<string>(type: "text", nullable: true),
                    location = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    personalEmail = table.Column<string>(type: "text", nullable: false),
                    portfolioLink = table.Column<string>(type: "text", nullable: true),
                    portfolioPdf = table.Column<string>(type: "text", nullable: true),
                    potentialLevel = table.Column<int>(type: "integer", nullable: false),
                    profilePicture = table.Column<string>(type: "text", nullable: true),
                    qualificationEndDate = table.Column<int>(type: "integer", nullable: true),
                    race = table.Column<int>(type: "integer", nullable: false),
                    referral = table.Column<int>(type: "integer", nullable: false),
                    school = table.Column<string>(type: "text", nullable: true),
                    surname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidate", x => x.id);
                });
        }
    }
}
