using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGO.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_projects_userId1",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_skill_skill_userId1",
                table: "skill");

            migrationBuilder.DropForeignKey(
                name: "FK_social_social_userId1",
                table: "social");

            migrationBuilder.DropIndex(
                name: "IX_skill_userId1",
                table: "skill");

            migrationBuilder.DropIndex(
                name: "IX_projects_userId1",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "skill");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "projects");

            migrationBuilder.AddForeignKey(
                name: "FK_social_users_userId1",
                table: "social",
                column: "userId1",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_projects_userId",
                table: "users",
                column: "userId",
                principalTable: "projects",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_users_skill_userId",
                table: "users",
                column: "userId",
                principalTable: "skill",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_social_users_userId1",
                table: "social");

            migrationBuilder.DropForeignKey(
                name: "FK_users_projects_userId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_skill_userId",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "userId1",
                table: "skill",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "userId1",
                table: "projects",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_skill_userId1",
                table: "skill",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_projects_userId1",
                table: "projects",
                column: "userId1");

            migrationBuilder.AddForeignKey(
                name: "FK_projects_projects_userId1",
                table: "projects",
                column: "userId1",
                principalTable: "projects",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_skill_skill_userId1",
                table: "skill",
                column: "userId1",
                principalTable: "skill",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_social_social_userId1",
                table: "social",
                column: "userId1",
                principalTable: "social",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
