using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class photo_type_string_to_byte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Employee\" ALTER COLUMN \"photo\" TYPE bytea USING \"photo\"::bytea;"
            );

            migrationBuilder.Sql(
                "ALTER TABLE \"Employee\" ALTER COLUMN \"photo\" SET DEFAULT '\\x'::bytea;"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE \"Employee\" ALTER COLUMN \"photo\" TYPE text USING \"photo\"::text;"
            );

            migrationBuilder.Sql(
                "ALTER TABLE \"Employee\" ALTER COLUMN \"photo\" DROP DEFAULT;"
            );
        }
    }
}