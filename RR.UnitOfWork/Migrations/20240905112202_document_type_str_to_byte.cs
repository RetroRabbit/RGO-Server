using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    public partial class document_type_str_to_byte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"EmployeeDocument\" ALTER COLUMN \"blob\" TYPE bytea USING \"blob\"::bytea;"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"EmployeeDocument\" ALTER COLUMN \"blob\" TYPE text USING \"blob\"::text;"
            );
        }
    }
}