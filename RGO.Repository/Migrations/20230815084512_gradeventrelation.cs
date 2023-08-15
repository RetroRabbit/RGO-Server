using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGO.Repository.Migrations
{
    /// <inheritdoc />
    public partial class gradeventrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_GradGroup_GradGroupId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_GradGroupId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "GradGroupId",
                table: "Events");

            migrationBuilder.CreateIndex(
                name: "IX_Events_gradGroupId",
                table: "Events",
                column: "gradGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_GradGroup_gradGroupId",
                table: "Events",
                column: "gradGroupId",
                principalTable: "GradGroup",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_GradGroup_gradGroupId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_gradGroupId",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "GradGroupId",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_GradGroupId",
                table: "Events",
                column: "GradGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_GradGroup_GradGroupId",
                table: "Events",
                column: "GradGroupId",
                principalTable: "GradGroup",
                principalColumn: "id");
        }
    }
}
