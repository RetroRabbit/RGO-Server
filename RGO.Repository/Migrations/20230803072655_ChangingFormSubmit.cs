using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGO.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangingFormSubmit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_users_certificateId",
                table: "Certifications");

            migrationBuilder.DropForeignKey(
                name: "FK_FormSubmit_input_formSubmitId",
                table: "FormSubmit");

            migrationBuilder.DropForeignKey(
                name: "FK_projects_users_projecId",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_skill_users_skillId",
                table: "skill");

            migrationBuilder.RenameColumn(
                name: "skillId",
                table: "skill",
                newName: "userId1");

            migrationBuilder.RenameIndex(
                name: "IX_skill_skillId",
                table: "skill",
                newName: "IX_skill_userId1");

            migrationBuilder.RenameColumn(
                name: "projecId",
                table: "projects",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_projects_projecId",
                table: "projects",
                newName: "IX_projects_userId");

            migrationBuilder.RenameColumn(
                name: "certificateId",
                table: "Certifications",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_Certifications_certificateId",
                table: "Certifications",
                newName: "IX_Certifications_userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_users_userId",
                table: "Certifications",
                column: "userId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormSubmit_FormSubmit_formSubmitId",
                table: "FormSubmit",
                column: "formSubmitId",
                principalTable: "FormSubmit",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_users_userId",
                table: "projects",
                column: "userId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_skill_users_userId1",
                table: "skill",
                column: "userId1",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_users_userId",
                table: "Certifications");

            migrationBuilder.DropForeignKey(
                name: "FK_FormSubmit_FormSubmit_formSubmitId",
                table: "FormSubmit");

            migrationBuilder.DropForeignKey(
                name: "FK_projects_users_userId",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_skill_users_userId1",
                table: "skill");

            migrationBuilder.RenameColumn(
                name: "userId1",
                table: "skill",
                newName: "skillId");

            migrationBuilder.RenameIndex(
                name: "IX_skill_userId1",
                table: "skill",
                newName: "IX_skill_skillId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "projects",
                newName: "projecId");

            migrationBuilder.RenameIndex(
                name: "IX_projects_userId",
                table: "projects",
                newName: "IX_projects_projecId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Certifications",
                newName: "certificateId");

            migrationBuilder.RenameIndex(
                name: "IX_Certifications_userId",
                table: "Certifications",
                newName: "IX_Certifications_certificateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_users_certificateId",
                table: "Certifications",
                column: "certificateId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_FormSubmit_input_formSubmitId",
                table: "FormSubmit",
                column: "formSubmitId",
                principalTable: "input",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_users_projecId",
                table: "projects",
                column: "projecId",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_skill_users_skillId",
                table: "skill",
                column: "skillId",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
