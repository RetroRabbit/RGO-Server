using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    public partial class termination_type_str_to_byte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Termination\" ALTER COLUMN \"terminationDocument\" TYPE bytea USING \"terminationDocument\"::bytea;"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Termination\" ALTER COLUMN \"terminationDocument\" TYPE text USING \"terminationDocument\"::text;"
            );
        }
    }
}
