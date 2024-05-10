using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RR.UnitOfWork.Migrations
{
    /// <inheritdoc />
    public partial class clientproject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Candidate",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    surname = table.Column<string>(type: "text", nullable: false),
                    personalEmail = table.Column<string>(type: "text", nullable: false),
                    potentialLevel = table.Column<int>(type: "integer", nullable: false),
                    jobPosition = table.Column<int>(type: "integer", nullable: false),
                    linkedIn = table.Column<string>(type: "text", nullable: true),
                    profilePicture = table.Column<string>(type: "text", nullable: true),
                    cellphone = table.Column<string>(type: "text", nullable: true),
                    location = table.Column<string>(type: "text", nullable: true),
                    cv = table.Column<string>(type: "text", nullable: true),
                    portfolioLink = table.Column<string>(type: "text", nullable: true),
                    portfolioPdf = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    race = table.Column<int>(type: "integer", nullable: false),
                    idNumber = table.Column<string>(type: "text", nullable: true),
                    referral = table.Column<int>(type: "integer", nullable: false),
                    highestQualification = table.Column<string>(type: "text", nullable: true),
                    school = table.Column<string>(type: "text", nullable: true),
                    qualificationEndDate = table.Column<int>(type: "integer", nullable: true),
                    blacklisted = table.Column<int>(type: "integer", nullable: false),
                    blacklistedReason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Chart",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<string>(type: "text", nullable: true),
                    dataTypes = table.Column<List<string>>(type: "text[]", nullable: true),
                    labels = table.Column<List<string>>(type: "text[]", nullable: true),
                    subType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chart", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAddress",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    unitNumber = table.Column<string>(type: "text", nullable: true),
                    complexName = table.Column<string>(type: "text", nullable: true),
                    streetName = table.Column<string>(type: "text", nullable: true),
                    streetNumber = table.Column<string>(type: "text", nullable: true),
                    suburbOrDistrict = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "text", nullable: true),
                    country = table.Column<string>(type: "text", nullable: true),
                    province = table.Column<string>(type: "text", nullable: true),
                    postalCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAddress", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEvaluationTemplate",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvaluationTemplate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeType",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    dateOfIncident = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    stackTrace = table.Column<string>(type: "text", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FieldCode",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    regex = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    @internal = table.Column<bool>(name: "internal", type: "boolean", nullable: false),
                    internalTable = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<int>(type: "integer", nullable: false),
                    required = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldCode", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyEmployeeTotal",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeTotal = table.Column<int>(type: "integer", nullable: false),
                    developerTotal = table.Column<int>(type: "integer", nullable: false),
                    designerTotal = table.Column<int>(type: "integer", nullable: false),
                    scrumMasterTotal = table.Column<int>(type: "integer", nullable: false),
                    businessSupportTotal = table.Column<int>(type: "integer", nullable: false),
                    month = table.Column<string>(type: "text", nullable: true),
                    year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyEmployeeTotal", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "RoleAccess",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    permission = table.Column<string>(type: "text", nullable: true),
                    grouping = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAccess", x => x.id);
                });

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

            migrationBuilder.CreateTable(
                name: "EmployeeEvaluationTemplateItem",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    templateId = table.Column<int>(type: "integer", nullable: false),
                    section = table.Column<string>(type: "text", nullable: false),
                    question = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvaluationTemplateItem", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluationTemplateItem_EmployeeEvaluationTemplate_t~",
                        column: x => x.templateId,
                        principalTable: "EmployeeEvaluationTemplate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeNumber = table.Column<string>(type: "text", nullable: true),
                    taxNumber = table.Column<string>(type: "text", nullable: true),
                    engagementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    terminationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    peopleChampion = table.Column<int>(type: "integer", nullable: true),
                    disability = table.Column<bool>(type: "boolean", nullable: false),
                    disabilityNotes = table.Column<string>(type: "text", nullable: true),
                    level = table.Column<int>(type: "integer", nullable: true),
                    employeeTypeId = table.Column<int>(type: "integer", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    leaveInterval = table.Column<float>(type: "real", nullable: true),
                    salaryDays = table.Column<float>(type: "real", nullable: true),
                    payRate = table.Column<float>(type: "real", nullable: true),
                    salary = table.Column<int>(type: "integer", nullable: true),
                    name = table.Column<string>(type: "text", nullable: true),
                    initials = table.Column<string>(type: "text", nullable: true),
                    surname = table.Column<string>(type: "text", nullable: true),
                    dateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    countryOfBirth = table.Column<string>(type: "text", nullable: true),
                    nationality = table.Column<string>(type: "text", nullable: true),
                    idNumber = table.Column<string>(type: "text", nullable: true),
                    passportNumber = table.Column<string>(type: "text", nullable: true),
                    passportExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    passportCountryIssue = table.Column<string>(type: "text", nullable: true),
                    race = table.Column<int>(type: "integer", nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: true),
                    photo = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    personalEmail = table.Column<string>(type: "text", nullable: true),
                    cellphoneNo = table.Column<string>(type: "text", nullable: true),
                    clientAllocated = table.Column<int>(type: "integer", nullable: true),
                    teamLead = table.Column<int>(type: "integer", nullable: true),
                    physicalAddress = table.Column<int>(type: "integer", nullable: true),
                    postalAddress = table.Column<int>(type: "integer", nullable: true),
                    houseNo = table.Column<string>(type: "text", nullable: true),
                    emergencyContactName = table.Column<string>(type: "text", nullable: true),
                    emergencyContactNo = table.Column<string>(type: "text", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    inactiveReason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.id);
                    table.ForeignKey(
                        name: "FK_Employee_Client_clientAllocated",
                        column: x => x.clientAllocated,
                        principalTable: "Client",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Employee_EmployeeAddress_physicalAddress",
                        column: x => x.physicalAddress,
                        principalTable: "EmployeeAddress",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Employee_EmployeeAddress_postalAddress",
                        column: x => x.postalAddress,
                        principalTable: "EmployeeAddress",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Employee_EmployeeType_employeeTypeId",
                        column: x => x.employeeTypeId,
                        principalTable: "EmployeeType",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Employee_Employee_peopleChampion",
                        column: x => x.peopleChampion,
                        principalTable: "Employee",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Employee_Employee_teamLead",
                        column: x => x.teamLead,
                        principalTable: "Employee",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "FieldCodeOptions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fieldCodeId = table.Column<int>(type: "integer", nullable: false),
                    option = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldCodeOptions", x => x.id);
                    table.ForeignKey(
                        name: "FK_FieldCodeOptions_FieldCode_fieldCodeId",
                        column: x => x.fieldCodeId,
                        principalTable: "FieldCode",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChartRoleLink",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleId = table.Column<int>(type: "integer", nullable: false),
                    chartId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartRoleLink", x => x.id);
                    table.ForeignKey(
                        name: "FK_ChartRoleLink_Chart_chartId",
                        column: x => x.chartId,
                        principalTable: "Chart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChartRoleLink_Role_roleId",
                        column: x => x.roleId,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyAccess",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleId = table.Column<int>(type: "integer", nullable: false),
                    Table = table.Column<string>(type: "text", nullable: false),
                    Field = table.Column<string>(type: "text", nullable: false),
                    AccessLevel = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyAccess", x => x.id);
                    table.ForeignKey(
                        name: "FK_PropertyAccess_Role_roleId",
                        column: x => x.roleId,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleAccessLink",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleId = table.Column<int>(type: "integer", nullable: false),
                    roleAccessId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleAccessLink", x => x.id);
                    table.ForeignKey(
                        name: "FK_RoleAccessLink_RoleAccess_roleAccessId",
                        column: x => x.roleAccessId,
                        principalTable: "RoleAccess",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleAccessLink_Role_roleId",
                        column: x => x.roleId,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CRUDOperation = table.Column<int>(type: "integer", nullable: false),
                    createdById = table.Column<int>(type: "integer", nullable: false),
                    table = table.Column<string>(type: "text", nullable: true),
                    data = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Employee_createdById",
                        column: x => x.createdById,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientProject",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nameOfClient = table.Column<string>(type: "text", nullable: true),
                    projectName = table.Column<string>(type: "text", nullable: true),
                    startDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    endDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    uploadProjectUrl = table.Column<string>(type: "text", nullable: true),
                    employeeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProject", x => x.id);
                    table.ForeignKey(
                        name: "FK_ClientProject_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeBanking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    bankName = table.Column<string>(type: "text", nullable: true),
                    branch = table.Column<string>(type: "text", nullable: true),
                    accountNo = table.Column<string>(type: "text", nullable: true),
                    accountType = table.Column<int>(type: "integer", nullable: false),
                    accountHolderName = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    file = table.Column<string>(type: "text", nullable: true),
                    lastUpdateDate = table.Column<DateOnly>(type: "date", nullable: false),
                    pendingUpdateDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeBanking", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeBanking_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeData",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    fieldCodeId = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeData", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeData_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeData_FieldCode_fieldCodeId",
                        column: x => x.fieldCodeId,
                        principalTable: "FieldCode",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDate",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: true),
                    note = table.Column<string>(type: "text", nullable: true),
                    date = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDate", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeDate_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDocument",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    reference = table.Column<string>(type: "text", nullable: true),
                    fileName = table.Column<string>(type: "text", nullable: true),
                    fileCategory = table.Column<int>(type: "integer", nullable: false),
                    blob = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true),
                    uploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    reason = table.Column<string>(type: "text", nullable: true),
                    counterSign = table.Column<bool>(type: "boolean", nullable: false),
                    documentType = table.Column<int>(type: "integer", nullable: true),
                    lastUpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDocument", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeDocument_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEvaluations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    templateId = table.Column<int>(type: "integer", nullable: false),
                    ownerId = table.Column<int>(type: "integer", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: false),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false),
                    endDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvaluations", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluations_EmployeeEvaluationTemplate_templateId",
                        column: x => x.templateId,
                        principalTable: "EmployeeEvaluationTemplate",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluations_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluations_Employee_ownerId",
                        column: x => x.ownerId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProjects",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    client = table.Column<string>(type: "text", nullable: true),
                    startDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    endDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProjects", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeProjects_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRole",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    roleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRole", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeRole_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRole_Role_roleId",
                        column: x => x.roleId,
                        principalTable: "Role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkExperience",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    employmentType = table.Column<string>(type: "text", nullable: true),
                    companyName = table.Column<string>(type: "text", nullable: true),
                    location = table.Column<string>(type: "text", nullable: true),
                    startDate = table.Column<DateOnly>(type: "date", nullable: false),
                    endDate = table.Column<DateOnly>(type: "date", nullable: false),
                    employeeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperience", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkExperience_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEvaluationAudience",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeEvaluationId = table.Column<int>(type: "integer", nullable: false),
                    employeeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvaluationAudience", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluationAudience_EmployeeEvaluations_employeeEval~",
                        column: x => x.employeeEvaluationId,
                        principalTable: "EmployeeEvaluations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluationAudience_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeEvaluationRatings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employeeEvaluationId = table.Column<int>(type: "integer", nullable: false),
                    employeeId = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    score = table.Column<float>(type: "real", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeEvaluationRatings", x => x.id);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluationRatings_EmployeeEvaluations_employeeEvalu~",
                        column: x => x.employeeEvaluationId,
                        principalTable: "EmployeeEvaluations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeEvaluationRatings_Employee_employeeId",
                        column: x => x.employeeId,
                        principalTable: "Employee",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_createdById",
                table: "AuditLogs",
                column: "createdById");

            migrationBuilder.CreateIndex(
                name: "IX_ChartDataSet_chartId",
                table: "ChartDataSet",
                column: "chartId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartRoleLink_chartId",
                table: "ChartRoleLink",
                column: "chartId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartRoleLink_roleId",
                table: "ChartRoleLink",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProject_employeeId",
                table: "ClientProject",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_clientAllocated",
                table: "Employee",
                column: "clientAllocated");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_employeeTypeId",
                table: "Employee",
                column: "employeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_peopleChampion",
                table: "Employee",
                column: "peopleChampion");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_physicalAddress",
                table: "Employee",
                column: "physicalAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_postalAddress",
                table: "Employee",
                column: "postalAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_teamLead",
                table: "Employee",
                column: "teamLead");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBanking_employeeId",
                table: "EmployeeBanking",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeData_employeeId",
                table: "EmployeeData",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeData_fieldCodeId",
                table: "EmployeeData",
                column: "fieldCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDate_employeeId",
                table: "EmployeeDate",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDocument_employeeId",
                table: "EmployeeDocument",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluationAudience_employeeEvaluationId",
                table: "EmployeeEvaluationAudience",
                column: "employeeEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluationAudience_employeeId",
                table: "EmployeeEvaluationAudience",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluationRatings_employeeEvaluationId",
                table: "EmployeeEvaluationRatings",
                column: "employeeEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluationRatings_employeeId",
                table: "EmployeeEvaluationRatings",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluations_employeeId",
                table: "EmployeeEvaluations",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluations_ownerId",
                table: "EmployeeEvaluations",
                column: "ownerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluations_templateId",
                table: "EmployeeEvaluations",
                column: "templateId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeEvaluationTemplateItem_templateId",
                table: "EmployeeEvaluationTemplateItem",
                column: "templateId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProjects_employeeId",
                table: "EmployeeProjects",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRole_employeeId",
                table: "EmployeeRole",
                column: "employeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRole_roleId",
                table: "EmployeeRole",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_FieldCodeOptions_fieldCodeId",
                table: "FieldCodeOptions",
                column: "fieldCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAccess_roleId",
                table: "PropertyAccess",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAccessLink_roleAccessId",
                table: "RoleAccessLink",
                column: "roleAccessId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleAccessLink_roleId",
                table: "RoleAccessLink",
                column: "roleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperience_employeeId",
                table: "WorkExperience",
                column: "employeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.DropTable(
                name: "ChartDataSet");

            migrationBuilder.DropTable(
                name: "ChartRoleLink");

            migrationBuilder.DropTable(
                name: "ClientProject");

            migrationBuilder.DropTable(
                name: "EmployeeBanking");

            migrationBuilder.DropTable(
                name: "EmployeeData");

            migrationBuilder.DropTable(
                name: "EmployeeDate");

            migrationBuilder.DropTable(
                name: "EmployeeDocument");

            migrationBuilder.DropTable(
                name: "EmployeeEvaluationAudience");

            migrationBuilder.DropTable(
                name: "EmployeeEvaluationRatings");

            migrationBuilder.DropTable(
                name: "EmployeeEvaluationTemplateItem");

            migrationBuilder.DropTable(
                name: "EmployeeProjects");

            migrationBuilder.DropTable(
                name: "EmployeeRole");

            migrationBuilder.DropTable(
                name: "ErrorLog");

            migrationBuilder.DropTable(
                name: "FieldCodeOptions");

            migrationBuilder.DropTable(
                name: "MonthlyEmployeeTotal");

            migrationBuilder.DropTable(
                name: "PropertyAccess");

            migrationBuilder.DropTable(
                name: "RoleAccessLink");

            migrationBuilder.DropTable(
                name: "WorkExperience");

            migrationBuilder.DropTable(
                name: "Chart");

            migrationBuilder.DropTable(
                name: "EmployeeEvaluations");

            migrationBuilder.DropTable(
                name: "FieldCode");

            migrationBuilder.DropTable(
                name: "RoleAccess");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "EmployeeEvaluationTemplate");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "EmployeeAddress");

            migrationBuilder.DropTable(
                name: "EmployeeType");
        }
    }
}
