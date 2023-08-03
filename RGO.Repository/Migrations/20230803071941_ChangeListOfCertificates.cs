using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGO.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangeListOfCertificates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_Certifications_userId",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "certificateId",
                table: "Certifications",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_certificateId",
                table: "Certifications",
                column: "certificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_users_certificateId",
                table: "Certifications",
                column: "certificateId",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_users_certificateId",
                table: "Certifications");

            migrationBuilder.DropIndex(
                name: "IX_Certifications_certificateId",
                table: "Certifications");

            migrationBuilder.DropColumn(
                name: "certificateId",
                table: "Certifications");

            migrationBuilder.AddForeignKey(
                name: "FK_users_Certifications_userId",
                table: "users",
                column: "userId",
                principalTable: "Certifications",
                principalColumn: "id");
        }
    }
}
