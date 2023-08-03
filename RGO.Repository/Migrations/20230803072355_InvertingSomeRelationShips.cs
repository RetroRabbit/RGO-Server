using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGO.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InvertingSomeRelationShips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_projects_userId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_users_skill_userId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_userId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "skillId",
                table: "skill",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "projecId",
                table: "projects",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_skill_skillId",
                table: "skill",
                column: "skillId");

            migrationBuilder.CreateIndex(
                name: "IX_projects_projecId",
                table: "projects",
                column: "projecId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_projects_users_projecId",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_skill_users_skillId",
                table: "skill");

            migrationBuilder.DropIndex(
                name: "IX_skill_skillId",
                table: "skill");

            migrationBuilder.DropIndex(
                name: "IX_projects_projecId",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "skillId",
                table: "skill");

            migrationBuilder.DropColumn(
                name: "projecId",
                table: "projects");

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_userId",
                table: "users",
                column: "userId");

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
    }
}
