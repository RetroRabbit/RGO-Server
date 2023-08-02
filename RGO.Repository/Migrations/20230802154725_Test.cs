using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RGO.Repository.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Certifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    userId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certifications", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    userId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.id);
                    table.ForeignKey(
                        name: "FK_projects_projects_userId1",
                        column: x => x.userId1,
                        principalTable: "projects",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "skill",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    userId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_skill", x => x.id);
                    table.ForeignKey(
                        name: "FK_skill_skill_userId1",
                        column: x => x.userId1,
                        principalTable: "skill",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "social",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    discord = table.Column<string>(type: "text", nullable: false),
                    codeWars = table.Column<string>(type: "text", nullable: false),
                    gitHub = table.Column<string>(type: "text", nullable: false),
                    linkedIn = table.Column<string>(type: "text", nullable: false),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    userId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_social", x => x.id);
                    table.ForeignKey(
                        name: "FK_social_social_userId1",
                        column: x => x.userId1,
                        principalTable: "social",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "stacks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    stackType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stacks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    groupId = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    userType = table.Column<int>(type: "integer", nullable: false),
                    startDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    endDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    eventType = table.Column<int>(type: "integer", nullable: false),
                    groupId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.id);
                    table.ForeignKey(
                        name: "FK_Events_UserGroup_groupId1",
                        column: x => x.groupId1,
                        principalTable: "UserGroup",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Form",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    groupId = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    startDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    endDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    groupId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Form", x => x.id);
                    table.ForeignKey(
                        name: "FK_Form_UserGroup_groupId1",
                        column: x => x.groupId1,
                        principalTable: "UserGroup",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gradGroupId = table.Column<int>(type: "integer", nullable: true),
                    firstName = table.Column<string>(type: "text", nullable: false),
                    lastName = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    joinDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    gradGroupId1 = table.Column<int>(type: "integer", nullable: false),
                    userId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_Certifications_userId",
                        column: x => x.userId,
                        principalTable: "Certifications",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_users_UserGroup_gradGroupId1",
                        column: x => x.gradGroupId1,
                        principalTable: "UserGroup",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Workshop",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    eventId = table.Column<int>(type: "integer", nullable: false),
                    presenter = table.Column<string>(type: "text", nullable: false),
                    eventId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshop", x => x.id);
                    table.ForeignKey(
                        name: "FK_Workshop_Events_eventId1",
                        column: x => x.eventId1,
                        principalTable: "Events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Field",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    formId = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    required = table.Column<bool>(type: "boolean", nullable: false),
                    label = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    errorMessage = table.Column<string>(type: "text", nullable: false),
                    formId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Field", x => x.id);
                    table.ForeignKey(
                        name: "FK_Field_Form_formId1",
                        column: x => x.formId1,
                        principalTable: "Form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStacks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    backendId = table.Column<int>(type: "integer", nullable: false),
                    frontendId = table.Column<int>(type: "integer", nullable: false),
                    databaseId = table.Column<int>(type: "integer", nullable: false),
                    createDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    userId1 = table.Column<int>(type: "integer", nullable: true),
                    backendId1 = table.Column<int>(type: "integer", nullable: false),
                    frontendId1 = table.Column<int>(type: "integer", nullable: false),
                    databaseId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStacks", x => x.id);
                    table.ForeignKey(
                        name: "FK_UserStacks_stacks_backendId1",
                        column: x => x.backendId1,
                        principalTable: "stacks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStacks_stacks_databaseId1",
                        column: x => x.databaseId1,
                        principalTable: "stacks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStacks_stacks_frontendId1",
                        column: x => x.frontendId1,
                        principalTable: "stacks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStacks_users_userId1",
                        column: x => x.userId1,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "input",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    formSubmitId = table.Column<int>(type: "integer", nullable: false),
                    fieldId = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    createDate = table.Column<int>(type: "integer", nullable: false),
                    fieldId1 = table.Column<int>(type: "integer", nullable: false),
                    userId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_input", x => x.id);
                    table.ForeignKey(
                        name: "FK_input_Field_fieldId1",
                        column: x => x.fieldId1,
                        principalTable: "Field",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_input_users_userId1",
                        column: x => x.userId1,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "options",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fieldId = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    fieldId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_options", x => x.id);
                    table.ForeignKey(
                        name: "FK_options_Field_fieldId1",
                        column: x => x.fieldId1,
                        principalTable: "Field",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FormSubmit",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    formid = table.Column<int>(type: "integer", nullable: false),
                    createDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    rejectionReason = table.Column<string>(type: "text", nullable: false),
                    userId1 = table.Column<int>(type: "integer", nullable: false),
                    formId = table.Column<int>(type: "integer", nullable: false),
                    formSubmitId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormSubmit", x => x.id);
                    table.ForeignKey(
                        name: "FK_FormSubmit_Form_formId",
                        column: x => x.formId,
                        principalTable: "Form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FormSubmit_input_formSubmitId",
                        column: x => x.formSubmitId,
                        principalTable: "input",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_FormSubmit_users_userId1",
                        column: x => x.userId1,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_groupId1",
                table: "Events",
                column: "groupId1");

            migrationBuilder.CreateIndex(
                name: "IX_Field_formId1",
                table: "Field",
                column: "formId1");

            migrationBuilder.CreateIndex(
                name: "IX_Form_groupId1",
                table: "Form",
                column: "groupId1");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmit_formId",
                table: "FormSubmit",
                column: "formId");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmit_formSubmitId",
                table: "FormSubmit",
                column: "formSubmitId");

            migrationBuilder.CreateIndex(
                name: "IX_FormSubmit_userId1",
                table: "FormSubmit",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_input_fieldId1",
                table: "input",
                column: "fieldId1");

            migrationBuilder.CreateIndex(
                name: "IX_input_userId1",
                table: "input",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_options_fieldId1",
                table: "options",
                column: "fieldId1");

            migrationBuilder.CreateIndex(
                name: "IX_projects_userId1",
                table: "projects",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_skill_userId1",
                table: "skill",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_social_userId1",
                table: "social",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_users_gradGroupId1",
                table: "users",
                column: "gradGroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_users_userId",
                table: "users",
                column: "userId");

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
                name: "IX_Workshop_eventId1",
                table: "Workshop",
                column: "eventId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FormSubmit");

            migrationBuilder.DropTable(
                name: "options");

            migrationBuilder.DropTable(
                name: "projects");

            migrationBuilder.DropTable(
                name: "skill");

            migrationBuilder.DropTable(
                name: "social");

            migrationBuilder.DropTable(
                name: "UserStacks");

            migrationBuilder.DropTable(
                name: "Workshop");

            migrationBuilder.DropTable(
                name: "input");

            migrationBuilder.DropTable(
                name: "stacks");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Field");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "Form");

            migrationBuilder.DropTable(
                name: "Certifications");

            migrationBuilder.DropTable(
                name: "UserGroup");
        }
    }
}
