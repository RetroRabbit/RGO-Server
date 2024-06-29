using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class addErrorlogFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ipAddress",
                table: "ErrorLog",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "requestBody",
                table: "ErrorLog",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "requestContentType",
                table: "ErrorLog",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "requestMethod",
                table: "ErrorLog",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "requestUrl",
                table: "ErrorLog",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ipAddress",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "requestBody",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "requestContentType",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "requestMethod",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "requestUrl",
                table: "ErrorLog");
        }
    }
}
