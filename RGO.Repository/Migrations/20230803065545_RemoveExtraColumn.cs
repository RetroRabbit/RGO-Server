using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGO.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExtraColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_UserGroup_gradGroupId1",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_gradGroupId1",
                table: "users");

            migrationBuilder.DropColumn(
                name: "gradGroupId1",
                table: "users");

            migrationBuilder.AlterColumn<int>(
                name: "gradGroupId",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_gradGroupId",
                table: "users",
                column: "gradGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_UserGroup_gradGroupId",
                table: "users",
                column: "gradGroupId",
                principalTable: "UserGroup",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_UserGroup_gradGroupId",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_users_gradGroupId",
                table: "users");

            migrationBuilder.AlterColumn<int>(
                name: "gradGroupId",
                table: "users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "gradGroupId1",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_users_gradGroupId1",
                table: "users",
                column: "gradGroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_users_UserGroup_gradGroupId1",
                table: "users",
                column: "gradGroupId1",
                principalTable: "UserGroup",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
