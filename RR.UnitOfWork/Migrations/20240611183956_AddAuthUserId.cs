using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "authuserid",
                table: "Employee",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "authuserid",
                table: "Employee");
        }
    }
}
