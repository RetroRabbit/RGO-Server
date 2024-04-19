using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class ChartDataSetAlteration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data",
                table: "Chart");

            migrationBuilder.CreateTable(
                name: "ChartDataSet",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    label = table.Column<string>(type: "text", nullable: true),
                    data = table.Column<List<int>>(type: "integer[]", nullable: true),
                    chartId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartDataSet", x => x.id);
                    table.ForeignKey(
                        name: "FK_ChartDataSet_Chart_chartId",
                        column: x => x.chartId,
                        principalTable: "Chart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChartDataSet_chartId",
                table: "ChartDataSet",
                column: "chartId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChartDataSet");

            migrationBuilder.AddColumn<List<int>>(
                name: "data",
                table: "Chart",
                type: "integer[]",
                nullable: true);
        }
    }
}
