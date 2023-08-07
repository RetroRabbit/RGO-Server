using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RGO.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddingToUserStack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "UserStacks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "status",
                table: "UserStacks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "UserStacks");

            migrationBuilder.DropColumn(
                name: "status",
                table: "UserStacks");
        }
    }
}
