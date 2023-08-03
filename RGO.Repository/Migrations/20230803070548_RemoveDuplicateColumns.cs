using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGO.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDuplicateColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_UserGroup_groupId1",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Field_Form_formId1",
                table: "Field");

            migrationBuilder.DropForeignKey(
                name: "FK_Form_UserGroup_groupId1",
                table: "Form");

            migrationBuilder.DropForeignKey(
                name: "FK_FormSubmit_Form_formId",
                table: "FormSubmit");

            migrationBuilder.DropForeignKey(
                name: "FK_FormSubmit_users_userId1",
                table: "FormSubmit");

            migrationBuilder.DropForeignKey(
                name: "FK_input_Field_fieldId1",
                table: "input");

            migrationBuilder.DropForeignKey(
                name: "FK_input_users_userId1",
                table: "input");

            migrationBuilder.DropForeignKey(
                name: "FK_options_Field_fieldId1",
                table: "options");

            migrationBuilder.DropForeignKey(
                name: "FK_social_users_userId1",
                table: "social");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_stacks_backendId1",
                table: "UserStacks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_stacks_databaseId1",
                table: "UserStacks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_stacks_frontendId1",
                table: "UserStacks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStacks_users_userId1",
                table: "UserStacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Workshop_Events_eventId1",
                table: "Workshop");

            migrationBuilder.DropIndex(
                name: "IX_Workshop_eventId1",
                table: "Workshop");

            migrationBuilder.DropIndex(
                name: "IX_UserStacks_backendId1",
                table: "UserStacks");

            migrationBuilder.DropIndex(
                name: "IX_UserStacks_databaseId1",
                table: "UserStacks");

            migrationBuilder.DropIndex(
                name: "IX_UserStacks_frontendId1",
                table: "UserStacks");

            migrationBuilder.DropIndex(
                name: "IX_UserStacks_userId1",
                table: "UserStacks");

            migrationBuilder.DropIndex(
                name: "IX_social_userId1",
                table: "social");

            migrationBuilder.DropIndex(
                name: "IX_options_fieldId1",
                table: "options");

            migrationBuilder.DropIndex(
                name: "IX_input_fieldId1",
                table: "input");

            migrationBuilder.DropIndex(
                name: "IX_input_userId1",
                table: "input");

            migrationBuilder.DropIndex(
                name: "IX_FormSubmit_formId",
                table: "FormSubmit");

            migrationBuilder.DropIndex(
                name: "IX_FormSubmit_userId1",
                table: "FormSubmit");

            migrationBuilder.DropIndex(
                name: "IX_Form_groupId1",
                table: "Form");

            migrationBuilder.DropIndex(
                name: "IX_Field_formId1",
                table: "Field");

            migrationBuilder.DropIndex(
                name: "IX_Events_groupId1",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "eventId1",
                table: "Workshop");

            migrationBuilder.DropColumn(
                name: "backendId1",
                table: "UserStacks");

            migrationBuilder.DropColumn(
                name: "databaseId1",
                table: "UserStacks");

            migrationBuilder.DropColumn(
                name: "frontendId1",
                table: "UserStacks");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "UserStacks");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "social");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "fieldId1",
                table: "options");

            migrationBuilder.DropColumn(
                name: "fieldId1",
                table: "input");

            migrationBuilder.DropColumn(
                name: "formSubmitId",
                table: "input");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "input");

            migrationBuilder.DropColumn(
                name: "formId",
                table: "FormSubmit");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "FormSubmit");

            migrationBuilder.DropColumn(
                name: "groupId1",
                table: "Form");

            migrationBuilder.DropColumn(
                name: "formId1",
                table: "Field");

            migrationBuilder.DropColumn(
                name: "groupId1",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Certifications");

            migrationBuilder.RenameColumn(
                name: "formid",
                table: "FormSubmit",
                newName: "formId");

            migrationBuilder.CreateIndex(
                name: "IX_Workshop_eventId",
                table: "Workshop",
                column: "eventId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStacks_backendId",
                table: "UserStacks",
                column: "backendId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStacks_databaseId",
                table: "UserStacks",
                column: "databaseId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStacks_frontendId",
                table: "UserStacks",
                column: "frontendId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStacks_userId",
                table: "UserStacks",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_social_userId",
                table: "social",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_options_fieldId",
                table: "options",
                column: "fieldId");

            migrationBuilder.CreateIndex(
                name: "IX_input_fieldId",
                table: "input",
                column: "fieldId");

            migrationBuilder.CreateIndex(
                name: "IX_input_userId",
                table: "input",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmit_formId",
                table: "FormSubmit",
                column: "formId");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmit_userId",
                table: "FormSubmit",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_groupId",
                table: "Form",
                column: "groupId");

            migrationBuilder.CreateIndex(
                name: "IX_Field_formId",
                table: "Field",
                column: "formId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_groupId",
                table: "Events",
                column: "groupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_UserGroup_groupId",
                table: "Events",
                column: "groupId",
                principalTable: "UserGroup",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Form_formId",
                table: "Field",
                column: "formId",
                principalTable: "Form",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Form_UserGroup_groupId",
                table: "Form",
                column: "groupId",
                principalTable: "UserGroup",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormSubmit_Form_formId",
                table: "FormSubmit",
                column: "formId",
                principalTable: "Form",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormSubmit_users_userId",
                table: "FormSubmit",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_input_Field_fieldId",
                table: "input",
                column: "fieldId",
                principalTable: "Field",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_input_users_userId",
                table: "input",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_options_Field_fieldId",
                table: "options",
                column: "fieldId",
                principalTable: "Field",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Workshop_Events_eventId",
                table: "Workshop",
                column: "eventId",
                principalTable: "Events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_UserGroup_groupId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Field_Form_formId",
                table: "Field");

            migrationBuilder.DropForeignKey(
                name: "FK_Form_UserGroup_groupId",
                table: "Form");

            migrationBuilder.DropForeignKey(
                name: "FK_FormSubmit_Form_formId",
                table: "FormSubmit");

            migrationBuilder.DropForeignKey(
                name: "FK_FormSubmit_users_userId",
                table: "FormSubmit");

            migrationBuilder.DropForeignKey(
                name: "FK_input_Field_fieldId",
                table: "input");

            migrationBuilder.DropForeignKey(
                name: "FK_input_users_userId",
                table: "input");

            migrationBuilder.DropForeignKey(
                name: "FK_options_Field_fieldId",
                table: "options");

            migrationBuilder.DropForeignKey(
                name: "FK_social_users_userId",
                table: "social");

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

            migrationBuilder.DropForeignKey(
                name: "FK_Workshop_Events_eventId",
                table: "Workshop");

            migrationBuilder.DropIndex(
                name: "IX_Workshop_eventId",
                table: "Workshop");

            migrationBuilder.DropIndex(
                name: "IX_UserStacks_backendId",
                table: "UserStacks");

            migrationBuilder.DropIndex(
                name: "IX_UserStacks_databaseId",
                table: "UserStacks");

            migrationBuilder.DropIndex(
                name: "IX_UserStacks_frontendId",
                table: "UserStacks");

            migrationBuilder.DropIndex(
                name: "IX_UserStacks_userId",
                table: "UserStacks");

            migrationBuilder.DropIndex(
                name: "IX_social_userId",
                table: "social");

            migrationBuilder.DropIndex(
                name: "IX_options_fieldId",
                table: "options");

            migrationBuilder.DropIndex(
                name: "IX_input_fieldId",
                table: "input");

            migrationBuilder.DropIndex(
                name: "IX_input_userId",
                table: "input");

            migrationBuilder.DropIndex(
                name: "IX_FormSubmit_formId",
                table: "FormSubmit");

            migrationBuilder.DropIndex(
                name: "IX_FormSubmit_userId",
                table: "FormSubmit");

            migrationBuilder.DropIndex(
                name: "IX_Form_groupId",
                table: "Form");

            migrationBuilder.DropIndex(
                name: "IX_Field_formId",
                table: "Field");

            migrationBuilder.DropIndex(
                name: "IX_Events_groupId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "formId",
                table: "FormSubmit",
                newName: "formid");

            migrationBuilder.AddColumn<int>(
                name: "eventId1",
                table: "Workshop",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "backendId1",
                table: "UserStacks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "databaseId1",
                table: "UserStacks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "frontendId1",
                table: "UserStacks",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId1",
                table: "UserStacks",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "userId1",
                table: "social",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "fieldId1",
                table: "options",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "fieldId1",
                table: "input",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "formSubmitId",
                table: "input",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId1",
                table: "input",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "formId",
                table: "FormSubmit",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId1",
                table: "FormSubmit",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "groupId1",
                table: "Form",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "formId1",
                table: "Field",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "groupId1",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "Certifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Workshop_eventId1",
                table: "Workshop",
                column: "eventId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserStacks_backendId1",
                table: "UserStacks",
                column: "backendId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserStacks_databaseId1",
                table: "UserStacks",
                column: "databaseId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserStacks_frontendId1",
                table: "UserStacks",
                column: "frontendId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserStacks_userId1",
                table: "UserStacks",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_social_userId1",
                table: "social",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_options_fieldId1",
                table: "options",
                column: "fieldId1");

            migrationBuilder.CreateIndex(
                name: "IX_input_fieldId1",
                table: "input",
                column: "fieldId1");

            migrationBuilder.CreateIndex(
                name: "IX_input_userId1",
                table: "input",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmit_formId",
                table: "FormSubmit",
                column: "formId");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmit_userId1",
                table: "FormSubmit",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_Form_groupId1",
                table: "Form",
                column: "groupId1");

            migrationBuilder.CreateIndex(
                name: "IX_Field_formId1",
                table: "Field",
                column: "formId1");

            migrationBuilder.CreateIndex(
                name: "IX_Events_groupId1",
                table: "Events",
                column: "groupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_UserGroup_groupId1",
                table: "Events",
                column: "groupId1",
                principalTable: "UserGroup",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Field_Form_formId1",
                table: "Field",
                column: "formId1",
                principalTable: "Form",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Form_UserGroup_groupId1",
                table: "Form",
                column: "groupId1",
                principalTable: "UserGroup",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormSubmit_Form_formId",
                table: "FormSubmit",
                column: "formId",
                principalTable: "Form",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FormSubmit_users_userId1",
                table: "FormSubmit",
                column: "userId1",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_input_Field_fieldId1",
                table: "input",
                column: "fieldId1",
                principalTable: "Field",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_input_users_userId1",
                table: "input",
                column: "userId1",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_options_Field_fieldId1",
                table: "options",
                column: "fieldId1",
                principalTable: "Field",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_social_users_userId1",
                table: "social",
                column: "userId1",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_stacks_backendId1",
                table: "UserStacks",
                column: "backendId1",
                principalTable: "stacks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_stacks_databaseId1",
                table: "UserStacks",
                column: "databaseId1",
                principalTable: "stacks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_stacks_frontendId1",
                table: "UserStacks",
                column: "frontendId1",
                principalTable: "stacks",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserStacks_users_userId1",
                table: "UserStacks",
                column: "userId1",
                principalTable: "users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Workshop_Events_eventId1",
                table: "Workshop",
                column: "eventId1",
                principalTable: "Events",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
