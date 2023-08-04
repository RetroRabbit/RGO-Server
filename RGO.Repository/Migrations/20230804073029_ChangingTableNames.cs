using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGO.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangingTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_users_userId",
                table: "Certifications");

            migrationBuilder.DropForeignKey(
                name: "FK_projects_users_userId",
                table: "projects");

            migrationBuilder.DropForeignKey(
                name: "FK_skill_users_userId",
                table: "skill");

            migrationBuilder.DropForeignKey(
                name: "FK_social_users_userId",
                table: "social");

            migrationBuilder.DropForeignKey(
                name: "FK_users_UserGroup_gradGroupId",
                table: "users");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_stacks_backendId",
                table: "UserStacks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_stacks_databaseId",
                table: "UserStacks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_stacks_frontendId",
                table: "UserStacks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_users_userId",
                table: "UserStacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stacks",
                table: "stacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_social",
                table: "social");

            migrationBuilder.DropPrimaryKey(
                name: "PK_skill",
                table: "skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_projects",
                table: "projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.RenameTable(
                name: "stacks",
                newName: "Stacks");

            migrationBuilder.RenameTable(
                name: "social",
                newName: "Social");

            migrationBuilder.RenameTable(
                name: "skill",
                newName: "Skill");

            migrationBuilder.RenameTable(
                name: "projects",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "IX_social_userId",
                table: "Social",
                newName: "IX_Social_userId");

            migrationBuilder.RenameIndex(
                name: "IX_skill_userId",
                table: "Skill",
                newName: "IX_Skill_userId");

            migrationBuilder.RenameIndex(
                name: "IX_projects_userId",
                table: "Projects",
                newName: "IX_Projects_userId");

            migrationBuilder.RenameIndex(
                name: "IX_users_gradGroupId",
                table: "User",
                newName: "IX_User_gradGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stacks",
                table: "Stacks",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Social",
                table: "Social",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill",
                table: "Skill",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_User_userId",
                table: "Certifications",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_User_userId",
                table: "Projects",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_User_userId",
                table: "Skill",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Social_User_userId",
                table: "Social",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_UserGroup_gradGroupId",
                table: "User",
                column: "gradGroupId",
                principalTable: "UserGroup",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_Stacks_backendId",
                table: "UserStacks",
                column: "backendId",
                principalTable: "Stacks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_Stacks_databaseId",
                table: "UserStacks",
                column: "databaseId",
                principalTable: "Stacks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_Stacks_frontendId",
                table: "UserStacks",
                column: "frontendId",
                principalTable: "Stacks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_User_userId",
                table: "UserStacks",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certifications_User_userId",
                table: "Certifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_User_userId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_User_userId",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_Social_User_userId",
                table: "Social");

            migrationBuilder.DropForeignKey(
                name: "FK_User_UserGroup_gradGroupId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_Stacks_backendId",
                table: "UserStacks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_Stacks_databaseId",
                table: "UserStacks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_Stacks_frontendId",
                table: "UserStacks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_User_userId",
                table: "UserStacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stacks",
                table: "Stacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Social",
                table: "Social");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.RenameTable(
                name: "Stacks",
                newName: "stacks");

            migrationBuilder.RenameTable(
                name: "Social",
                newName: "social");

            migrationBuilder.RenameTable(
                name: "Skill",
                newName: "skill");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "projects");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "users");

            migrationBuilder.RenameIndex(
                name: "IX_Social_userId",
                table: "social",
                newName: "IX_social_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Skill_userId",
                table: "skill",
                newName: "IX_skill_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_userId",
                table: "projects",
                newName: "IX_projects_userId");

            migrationBuilder.RenameIndex(
                name: "IX_User_gradGroupId",
                table: "users",
                newName: "IX_users_gradGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stacks",
                table: "stacks",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_social",
                table: "social",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_skill",
                table: "skill",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_projects",
                table: "projects",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Certifications_users_userId",
                table: "Certifications",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_projects_users_userId",
                table: "projects",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_skill_users_userId",
                table: "skill",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_social_users_userId",
                table: "social",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_users_UserGroup_gradGroupId",
                table: "users",
                column: "gradGroupId",
                principalTable: "UserGroup",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_stacks_backendId",
                table: "UserStacks",
                column: "backendId",
                principalTable: "stacks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_stacks_databaseId",
                table: "UserStacks",
                column: "databaseId",
                principalTable: "stacks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_stacks_frontendId",
                table: "UserStacks",
                column: "frontendId",
                principalTable: "stacks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_users_userId",
                table: "UserStacks",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
