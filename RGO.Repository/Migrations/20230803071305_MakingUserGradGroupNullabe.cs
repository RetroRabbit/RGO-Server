using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGO.Repository.Migrations
{
    /// <inheritdoc />
    public partial class MakingUserGradGroupNullabe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_UserGroup_gradGroupId",
                table: "users");

            migrationBuilder.AlterColumn<int>(
                name: "gradGroupId",
                table: "users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_users_UserGroup_gradGroupId",
                table: "users",
                column: "gradGroupId",
                principalTable: "UserGroup",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_UserGroup_gradGroupId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_users_UserGroup_gradGroupId",
                table: "users",
                column: "gradGroupId",
                principalTable: "UserGroup",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
