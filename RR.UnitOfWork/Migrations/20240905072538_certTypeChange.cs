using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class certTypeChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"EmployeeCertification\" ALTER COLUMN \"certificateDocument\" TYPE bytea USING \"certificateDocument\"::bytea;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"EmployeeCertification\" ALTER COLUMN \"certificateDocument\" TYPE text USING \"certificateDocument\"::text;"
            );
        }
    }
}
