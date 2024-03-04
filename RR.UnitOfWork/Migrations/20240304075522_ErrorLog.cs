using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class ErrorLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeAddress_physicalAddress",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeAddress_postalAddress",
                table: "Employee");

            migrationBuilder.AlterColumn<string>(
                name: "permission",
                table: "RoleAccess",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "grouping",
                table: "RoleAccess",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Role",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "month",
                table: "MonthlyEmployeeTotal",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "option",
                table: "FieldCodeOptions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "FieldCode",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "FieldCode",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "EmployeeType",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "EmployeeProjects",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "EmployeeProjects",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "client",
                table: "EmployeeProjects",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "fileName",
                table: "EmployeeDocument",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "blob",
                table: "EmployeeDocument",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "subject",
                table: "EmployeeDate",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "EmployeeDate",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "reason",
                table: "EmployeeBanking",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "file",
                table: "EmployeeBanking",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "branch",
                table: "EmployeeBanking",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "bankName",
                table: "EmployeeBanking",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "accountNo",
                table: "EmployeeBanking",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "accountHolderName",
                table: "EmployeeBanking",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "unitNumber",
                table: "EmployeeAddress",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "suburbOrDistrict",
                table: "EmployeeAddress",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "streetNumber",
                table: "EmployeeAddress",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "province",
                table: "EmployeeAddress",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "postalCode",
                table: "EmployeeAddress",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "EmployeeAddress",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "complexName",
                table: "EmployeeAddress",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "city",
                table: "EmployeeAddress",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "surname",
                table: "Employee",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "postalAddress",
                table: "Employee",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "physicalAddress",
                table: "Employee",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "personalEmail",
                table: "Employee",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Employee",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "initials",
                table: "Employee",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "idNumber",
                table: "Employee",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Employee",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "disabilityNotes",
                table: "Employee",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "cellphoneNo",
                table: "Employee",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Client",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "Chart",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Chart",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<List<string>>(
                name: "labels",
                table: "Chart",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<List<string>>(
                name: "dataTypes",
                table: "Chart",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<List<int>>(
                name: "data",
                table: "Chart",
                type: "integer[]",
                nullable: true,
                oldClrType: typeof(List<int>),
                oldType: "integer[]");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "AuditLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dateOfIncident = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    exceptionType = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.id);
                });

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 1,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(617));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 2,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(768));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 3,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(785));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 4,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(795));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 5,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(811));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 6,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(821));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 7,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(829));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 8,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(838));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 9,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(847));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 10,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(857));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 11,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(871));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 12,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(880));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 13,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(888));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 14,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(909));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 15,
                column: "engagementDate",
                value: new DateTime(2024, 3, 4, 7, 55, 21, 821, DateTimeKind.Utc).AddTicks(901));

            migrationBuilder.UpdateData(
                table: "EmployeeEvaluations",
                keyColumn: "id",
                keyValue: 1,
                column: "startDate",
                value: new DateOnly(2024, 3, 4));

            migrationBuilder.UpdateData(
                table: "EmployeeEvaluations",
                keyColumn: "id",
                keyValue: 2,
                column: "startDate",
                value: new DateOnly(2024, 3, 4));

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeAddress_physicalAddress",
                table: "Employee",
                column: "physicalAddress",
                principalTable: "EmployeeAddress",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeAddress_postalAddress",
                table: "Employee",
                column: "postalAddress",
                principalTable: "EmployeeAddress",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeAddress_physicalAddress",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeAddress_postalAddress",
                table: "Employee");

            migrationBuilder.DropTable(
                name: "ErrorLog");

            migrationBuilder.AlterColumn<string>(
                name: "permission",
                table: "RoleAccess",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "grouping",
                table: "RoleAccess",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Role",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "month",
                table: "MonthlyEmployeeTotal",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "option",
                table: "FieldCodeOptions",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "FieldCode",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "FieldCode",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "EmployeeType",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "EmployeeProjects",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "EmployeeProjects",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "client",
                table: "EmployeeProjects",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "fileName",
                table: "EmployeeDocument",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "blob",
                table: "EmployeeDocument",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "subject",
                table: "EmployeeDate",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "note",
                table: "EmployeeDate",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "reason",
                table: "EmployeeBanking",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "file",
                table: "EmployeeBanking",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "branch",
                table: "EmployeeBanking",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "bankName",
                table: "EmployeeBanking",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "accountNo",
                table: "EmployeeBanking",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "accountHolderName",
                table: "EmployeeBanking",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "unitNumber",
                table: "EmployeeAddress",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "suburbOrDistrict",
                table: "EmployeeAddress",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "streetNumber",
                table: "EmployeeAddress",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "province",
                table: "EmployeeAddress",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "postalCode",
                table: "EmployeeAddress",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "EmployeeAddress",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "complexName",
                table: "EmployeeAddress",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "city",
                table: "EmployeeAddress",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "surname",
                table: "Employee",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "postalAddress",
                table: "Employee",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "physicalAddress",
                table: "Employee",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "personalEmail",
                table: "Employee",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Employee",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "initials",
                table: "Employee",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "idNumber",
                table: "Employee",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "Employee",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "disabilityNotes",
                table: "Employee",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cellphoneNo",
                table: "Employee",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Client",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "Chart",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Chart",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<string>>(
                name: "labels",
                table: "Chart",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<string>>(
                name: "dataTypes",
                table: "Chart",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<int>>(
                name: "data",
                table: "Chart",
                type: "integer[]",
                nullable: false,
                oldClrType: typeof(List<int>),
                oldType: "integer[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "AuditLogs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 1,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9190));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 2,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9304));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 3,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9318));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 4,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9327));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 5,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9344));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 6,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9352));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 7,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9360));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 8,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9368));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 9,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9376));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 10,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 11,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9392));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 12,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 13,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 14,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9445));

            migrationBuilder.UpdateData(
                table: "Employee",
                keyColumn: "id",
                keyValue: 15,
                column: "engagementDate",
                value: new DateTime(2024, 3, 1, 8, 42, 56, 720, DateTimeKind.Utc).AddTicks(9438));

            migrationBuilder.UpdateData(
                table: "EmployeeEvaluations",
                keyColumn: "id",
                keyValue: 1,
                column: "startDate",
                value: new DateOnly(2024, 3, 1));

            migrationBuilder.UpdateData(
                table: "EmployeeEvaluations",
                keyColumn: "id",
                keyValue: 2,
                column: "startDate",
                value: new DateOnly(2024, 3, 1));

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeAddress_physicalAddress",
                table: "Employee",
                column: "physicalAddress",
                principalTable: "EmployeeAddress",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeAddress_postalAddress",
                table: "Employee",
                column: "postalAddress",
                principalTable: "EmployeeAddress",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
