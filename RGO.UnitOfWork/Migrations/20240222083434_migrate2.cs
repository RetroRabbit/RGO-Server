using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class migrate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 1,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(3863));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 2,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4013));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 3,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4027));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 4,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4039));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 5,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4061));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 6,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4073));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 7,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4087));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 8,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4102));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 9,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4114));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 10,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4126));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 11,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4174));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 12,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4187));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 13,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4199));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 14,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4223));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 15,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 34, 34, 413, DateTimeKind.Utc).AddTicks(4211));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 1,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5212));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 2,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5379));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 3,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5394));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 4,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5407));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 5,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5428));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 6,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5441));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 7,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5459));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 8,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5473));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 9,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5485));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 10,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5498));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 11,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5511));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 12,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5523));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 13,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5566));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 14,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5589));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 15,
                column: "engagementDate",
                value: new DateTime(2024, 2, 22, 8, 33, 53, 622, DateTimeKind.Utc).AddTicks(5577));
        }
    }
}
