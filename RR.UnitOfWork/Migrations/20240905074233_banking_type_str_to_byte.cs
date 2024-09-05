using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    public partial class banking_type_str_to_byte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"EmployeeBanking\" ALTER COLUMN \"file\" TYPE bytea USING \"file\"::bytea;"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"EmployeeBanking\" ALTER COLUMN \"file\" TYPE text USING \"file\"::text;"
            );
        }
    }
}