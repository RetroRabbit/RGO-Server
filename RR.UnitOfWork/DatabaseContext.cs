using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.ATS;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork;

public interface IDatabaseContext
{
    DbSet<Employee> employees { get; set; }
    DbSet<EmployeeRole> employeeRoles { get; set; }
    DbSet<EmployeeAddress> employeeAddresses { get; set; }
    DbSet<EmployeeCertification> employeeCertification { get; set; }
    DbSet<EmployeeDocument> employeeDocuments { get; set; }
    DbSet<EmployeeProject> employeeProjects { get; set; }
    DbSet<EmployeeEvaluation> employeeEvaluations { get; set; }
    DbSet<EmployeeEvaluationAudience> employeeEvaluationAudiences { get; set; }
    DbSet<EmployeeEvaluationRating> employeeEvaluationRatings { get; set; }
    DbSet<EmployeeEvaluationTemplate> employeeEvaluationTemplates { get; set; }
    DbSet<EmployeeEvaluationTemplateItem> employeeEvaluationTemplateItem { get; set; }
    DbSet<EmployeeQualification> employeeQualifications { get; set; }
    DbSet<EmployeeSalaryDetails> employeeSalaryDetails { get; set; }
    DbSet<Role> roles { get; set; }
    DbSet<RoleAccess> roleAccess { get; set; }
    DbSet<AuditLog> auditLogs { get; set; }
    DbSet<Chart> Chart { get; set; }
    DbSet<ChartRoleLink> ChartRoleLink { get; set; }
    DbSet<FieldCode> fieldCodes { get; set; }
    DbSet<FieldCodeOptions> fieldCodesOptions { get; set; }
    DbSet<EmployeeData> employeeData { get; set; }
    DbSet<EmployeeDate> employeeDate { get; set; }
    DbSet<PropertyAccess> propertyAccesses { get; set; }
    DbSet<RoleAccessLink> roleAccessLinks { get; set; }
    DbSet<EmployeeBanking> employeeBanking { get; set; }
    DbSet<Client> clients { get; set; }
    DbSet<ClientProject> clientsProject { get; set; }
    DbSet<MonthlyEmployeeTotal> monthlyEmployeeTotal { get; set; }
    DbSet<ErrorLogging> errorLogging { get; set; }
    DbSet<Candidate> candidate { get; set; }
    DbSet<WorkExperience> workExperience { get; set; }
    DbSet<Termination> termination { get; set; }
    DbSet<DataReport> dataReport { get; set; }
}

public class DatabaseContext : DbContext, IDatabaseContext
{
    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    public DbSet<Employee> employees { get; set; }
    public DbSet<EmployeeRole> employeeRoles { get; set; }
    public DbSet<EmployeeAddress> employeeAddresses { get; set; }
    public DbSet<EmployeeCertification> employeeCertification { get; set; }
    public DbSet<EmployeeDocument> employeeDocuments { get; set; }
    public DbSet<EmployeeProject> employeeProjects { get; set; }
    public DbSet<EmployeeEvaluation> employeeEvaluations { get; set; }
    public DbSet<EmployeeEvaluationAudience> employeeEvaluationAudiences { get; set; }
    public DbSet<EmployeeEvaluationRating> employeeEvaluationRatings { get; set; }
    public DbSet<EmployeeEvaluationTemplate> employeeEvaluationTemplates { get; set; }
    public DbSet<EmployeeEvaluationTemplateItem> employeeEvaluationTemplateItem { get; set; }
    public DbSet<EmployeeQualification> employeeQualifications { get; set; }
    public DbSet<EmployeeSalaryDetails> employeeSalaryDetails { get; set; }
    public DbSet<Role> roles { get; set; }
    public DbSet<RoleAccess> roleAccess { get; set; }
    public DbSet<AuditLog> auditLogs { get; set; }
    public DbSet<Chart> Chart { get; set; }
    public DbSet<ChartRoleLink> ChartRoleLink { get; set; }
    public DbSet<FieldCode> fieldCodes { get; set; }
    public DbSet<FieldCodeOptions> fieldCodesOptions { get; set; }
    public DbSet<EmployeeData> employeeData { get; set; }
    public DbSet<EmployeeDate> employeeDate { get; set; }
    public DbSet<PropertyAccess> propertyAccesses { get; set; }
    public DbSet<RoleAccessLink> roleAccessLinks { get; set; }
    public DbSet<EmployeeBanking> employeeBanking { get; set; }
    public DbSet<Client> clients { get; set; }
    public DbSet<ClientProject> clientsProject { get; set; }
    public DbSet<MonthlyEmployeeTotal> monthlyEmployeeTotal { get; set; }
    public DbSet<ErrorLogging> errorLogging { get; set; }
    public DbSet<Candidate> candidate { get; set; }
    public DbSet<WorkExperience> workExperience { get; set; }
    public DbSet<Termination> termination { get; set; }
    public DbSet<DataReport> dataReport { get; set; }
    public DbSet<DataReportColumnMenu> dataReportColumnMenu { get; set; }
    public DbSet<DataReportColumns> dataReportColumns { get; set; }
    public DbSet<DataReportFilter> dataReportFilter { get; set; }
    public DbSet<DataReportValues> dataReportValues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__Default"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DataReport>().HasData(new DataReport
        {
            Id = 1,
            Name = "Availability Snapshot",
            Code = "AS01",
            Status = ItemStatus.Active
        });

        var sequence = 0;
        modelBuilder.Entity<DataReportColumns>().HasData(
            new DataReportColumns
            {
                Id = 1,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = 5,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Employee
            },
            new DataReportColumns
            {
                Id = 2,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = 7,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Employee
            },
            new DataReportColumns
            {
                Id = 3,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = 20,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Employee
            },
            new DataReportColumns
            {
                Id = 4,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = 4,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Employee
            },
            new DataReportColumns
            {
                Id = 5,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = 42,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Employee
            },
            new DataReportColumns
            {
                Id = 7,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = 43,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Employee
            },
            new DataReportColumns
            {
                Id = 8,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = 47,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Employee
            },
            new DataReportColumns
            {
                Id = 9,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = 49,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Employee
            },
            new DataReportColumns
            {
                Id = 10,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = 51,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Employee
            },
            new DataReportColumns
            {
                Id = 11,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = null,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Text,
                CustomName = "Internal Project",
                CustomProp = "InternalProject"
            },
            new DataReportColumns
            {
                Id = 12,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = null,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Text,
                CustomName = "Open Source",
                CustomProp = "OpenSource"
            },
            new DataReportColumns
            {
                Id = 13,
                Status = ItemStatus.Active,
                ReportId = 1,
                MenuId = null,
                Sequence = sequence++,
                FieldType = DataReportColumnType.Text,
                CustomName = "Notes",
                CustomProp = "Notes"
            }
        );

        modelBuilder.Entity<DataReportFilter>().HasData(
            new DataReportFilter
            {
                Id = 1,
                Status = ItemStatus.Active,
                ReportId = 1,
                Table = "Employee",
                Column = "clientAllocated",
                Condition = "IS NULL",
                Value = null,
                Select = "id",
            },
            new DataReportFilter
            {
                Id = 2,
                Status = ItemStatus.Active,
                ReportId = 1,
                Table = "Employee",
                Column = "employeeTypeId",
                Condition = "IN",
                Value = "(2,3,4)",
                Select = "id",
            }
        );

        modelBuilder.Entity<FieldCode>().HasData(
            new FieldCode { Id = 1, Code = "degree", Name = "Degree", Description = null, Regex = null, Type = (FieldCodeType)1, Status = (ItemStatus)0, Internal = false, InternalTable = null, Category = (FieldCodeCategory)0, Required = false },
            new FieldCode { Id = 2, Code = "tenure", Name = "Tenure", Description = null, Regex = null, Type = (FieldCodeType)1, Status = (ItemStatus)0, Internal = false, InternalTable = null, Category = (FieldCodeCategory)0, Required = false },
            new FieldCode { Id = 3, Code = "nqf", Name = "NQF Level", Description = null, Regex = null, Type = (FieldCodeType)4, Status = (ItemStatus)0, Internal = false, InternalTable = null, Category = (FieldCodeCategory)0, Required = false },
            new FieldCode { Id = 4, Code = "institution", Name = "Institution", Description = null, Regex = null, Type = (FieldCodeType)4, Status = (ItemStatus)0, Internal = false, InternalTable = null, Category = (FieldCodeCategory)0, Required = false },
            new FieldCode { Id = 5, Code = "experience", Name = "Experience", Description = null, Regex = null, Type = (FieldCodeType)4, Status = (ItemStatus)0, Internal = false, InternalTable = null, Category = (FieldCodeCategory)0, Required = false },
            new FieldCode { Id = 6, Code = "cv", Name = "CV Link", Description = null, Regex = null, Type = (FieldCodeType)1, Status = (ItemStatus)0, Internal = false, InternalTable = null, Category = (FieldCodeCategory)1, Required = false },
            new FieldCode { Id = 7, Code = "skills", Name = "Tech Stack", Description = null, Regex = null, Type = (FieldCodeType)1, Status = (ItemStatus)0, Internal = false, InternalTable = null, Category = (FieldCodeCategory)1, Required = false },
            new FieldCode { Id = 8, Code = "engagement", Name = "Engagement", Description = null, Regex = null, Type = (FieldCodeType)4, Status = (ItemStatus)0, Internal = false, InternalTable = null, Category = (FieldCodeCategory)1, Required = false },
            new FieldCode { Id = 9, Code = "location", Name = "Location", Description = null, Regex = null, Type = (FieldCodeType)4, Status = (ItemStatus)0, Internal = false, InternalTable = null, Category = (FieldCodeCategory)2, Required = false },
            new FieldCode { Id = 10, Code = "risk", Name = "Risk", Description = null, Regex = null, Type = (FieldCodeType)4, Status = (ItemStatus)0, Internal = false, InternalTable = null, Category = (FieldCodeCategory)2, Required = false }
        );

        modelBuilder.Entity<FieldCodeOptions>().HasData(
            new FieldCodeOptions { Id = 1, FieldCodeId = 10, Option = "Very Low" },
            new FieldCodeOptions { Id = 2, FieldCodeId = 10, Option = "Low" },
            new FieldCodeOptions { Id = 3, FieldCodeId = 10, Option = "Medium" },
            new FieldCodeOptions { Id = 4, FieldCodeId = 10, Option = "High" },
            new FieldCodeOptions { Id = 5, FieldCodeId = 10, Option = "Very High" },
            new FieldCodeOptions { Id = 6, FieldCodeId = 10, Option = "Unknown" },
            new FieldCodeOptions { Id = 7, FieldCodeId =  9, Option = "JHB" },
            new FieldCodeOptions { Id = 8, FieldCodeId =  9, Option = "PTA" },
            new FieldCodeOptions { Id = 9, FieldCodeId =  9, Option = "CP" },
            new FieldCodeOptions { Id = 10, FieldCodeId = 9, Option = "Other" },
            new FieldCodeOptions { Id = 11, FieldCodeId = 8, Option = "Very Low" },
            new FieldCodeOptions { Id = 12, FieldCodeId = 8, Option = "Low" },
            new FieldCodeOptions { Id = 13, FieldCodeId = 8, Option = "Average" },
            new FieldCodeOptions { Id = 14, FieldCodeId = 8, Option = "High" },
            new FieldCodeOptions { Id = 15, FieldCodeId = 8, Option = "Unknown" },
            new FieldCodeOptions { Id = 16, FieldCodeId = 5, Option = "Grad" },
            new FieldCodeOptions { Id = 17, FieldCodeId = 5, Option = "1+" },
            new FieldCodeOptions { Id = 18, FieldCodeId = 5, Option = "2+" },
            new FieldCodeOptions { Id = 19, FieldCodeId = 5, Option = "3+" },
            new FieldCodeOptions { Id = 20, FieldCodeId = 5, Option = "4+" },
            new FieldCodeOptions { Id = 21, FieldCodeId = 5, Option = "5+" },
            new FieldCodeOptions { Id = 22, FieldCodeId = 5, Option = "6+" },
            new FieldCodeOptions { Id = 23, FieldCodeId = 5, Option = "7+" },
            new FieldCodeOptions { Id = 24, FieldCodeId = 5, Option = "8+" },
            new FieldCodeOptions { Id = 25, FieldCodeId = 5, Option = "9+" },
            new FieldCodeOptions { Id = 26, FieldCodeId = 5, Option = "10+" },
            new FieldCodeOptions { Id = 27, FieldCodeId = 5, Option = "15+" },
            new FieldCodeOptions { Id = 28, FieldCodeId = 5, Option = "20+" },
            new FieldCodeOptions { Id = 29, FieldCodeId = 4, Option = "TUT" },
            new FieldCodeOptions { Id = 30, FieldCodeId = 4, Option = "Belgium Campus" },
            new FieldCodeOptions { Id = 31, FieldCodeId = 4, Option = "University of Limpopo" },
            new FieldCodeOptions { Id = 32, FieldCodeId = 4, Option = "University of Pretoria" },
            new FieldCodeOptions { Id = 33, FieldCodeId = 4, Option = "NWU - Potch" },
            new FieldCodeOptions { Id = 34, FieldCodeId = 4, Option = "Pearson" },
            new FieldCodeOptions { Id = 35, FieldCodeId = 4, Option = "University of Johannesburg" },
            new FieldCodeOptions { Id = 36, FieldCodeId = 4, Option = "Other" },
            new FieldCodeOptions { Id = 37, FieldCodeId = 4, Option = "Open Window" },
            new FieldCodeOptions { Id = 38, FieldCodeId = 4, Option = "University of Cape Town" },
            new FieldCodeOptions { Id = 39, FieldCodeId = 4, Option = "Rhodes" },
            new FieldCodeOptions { Id = 40, FieldCodeId = 4, Option = "University of Kwa-Zulu Natal" },
            new FieldCodeOptions { Id = 41, FieldCodeId = 4, Option = "UNISA" },
            new FieldCodeOptions { Id = 42, FieldCodeId = 4, Option = "University of Witwatersrand" },
            new FieldCodeOptions { Id = 43, FieldCodeId = 3, Option = "NQF 4" },
            new FieldCodeOptions { Id = 44, FieldCodeId = 3, Option = "NQF 5" },
            new FieldCodeOptions { Id = 45, FieldCodeId = 3, Option = "NQF 6" },
            new FieldCodeOptions { Id = 46, FieldCodeId = 3, Option = "NQF 7" },
            new FieldCodeOptions { Id = 47, FieldCodeId = 3, Option = "NQF 8" },
            new FieldCodeOptions { Id = 48, FieldCodeId = 3, Option = "NQF 9" },
            new FieldCodeOptions { Id = 49, FieldCodeId = 3, Option = "NQF 10" }
       );

        modelBuilder.Entity<DataReportColumnMenu>().HasData(
            new DataReportColumnMenu
            {
                Id = 1, Name = "Employee Number", Prop = "EmployeeNumber", Mapping = "EmployeeNumber", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 2, Name = "Engagement Date", Prop = "EngagementDate", Mapping = "EngagementDate", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 3, Name = "Termination Date", Prop = "TerminationDate", Mapping = "TerminationDate", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 4, Name = "Level", Prop = "Level", Mapping = "Level", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 5, Name = "Name", Prop = "Name", Mapping = "Name", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 6, Name = "Initials", Prop = "Initials", Mapping = "Initials", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 7, Name = "Surname", Prop = "Surname", Mapping = "Surname", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 8, Name = "Date Of Birth", Prop = "DateOfBirth", Mapping = "DateOfBirth", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 9, Name = "Country Of Birth", Prop = "CountryOfBirth", Mapping = "CountryOfBirth", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 10, Name = "Nationality", Prop = "Nationality", Mapping = "Nationality", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 11, Name = "Race", Prop = "Race", Mapping = "Race", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 12, Name = "Gender", Prop = "Gender", Mapping = "Gender", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 13, Name = "Email", Prop = "Email", Mapping = "Email", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 14, Name = "Personal Email", Prop = "PersonalEmail", Mapping = "PersonalEmail", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 15, Name = "Cellphone No", Prop = "CellphoneNo", Mapping = "CellphoneNo", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 16, Name = "House No", Prop = "HouseNo", Mapping = "HouseNo", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 17, Name = "Emergency Contact Name", Prop = "EmergencyContactName", Mapping = "EmergencyContactName", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 18, Name = "Emergency Contact No", Prop = "EmergencyContactNo", Mapping = "EmergencyContactNo", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 19, Name = "Inactive Reason", Prop = "InactiveReason", Mapping = "InactiveReason", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 20, Name = "Role", Prop = "Role", Mapping = "EmployeeType.Name", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 21, Name = "Client Assigned", Prop = "ClientAssigned", Mapping = "ClientAssigned.Name", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 22, Name = "Physical Address", Prop = "PhysicalAddress", Mapping = "PhysicalAddress", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 23, Name = "Unit Number", Prop = "UnitNumber", Mapping = "PhysicalAddress.UnitNumber", ParentId = 22, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 24, Name = "Complex Name", Prop = "ComplexName", Mapping = "PhysicalAddress.ComplexName", ParentId = 22, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 25, Name = "Street Name", Prop = "StreetName", Mapping = "PhysicalAddress.StreetName", ParentId = 22, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 26, Name = "Street Number", Prop = "StreetNumber", Mapping = "PhysicalAddress.StreetNumber", ParentId = 22, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 27, Name = "Suburb Or District", Prop = "SuburbOrDistrict", Mapping = "PhysicalAddress.SuburbOrDistrict", ParentId = 22, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 28, Name = "City", Prop = "City", Mapping = "PhysicalAddress.City", ParentId = 22, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 29, Name = "Country", Prop = "Country", Mapping = "PhysicalAddress.Country", ParentId = 22, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 30, Name = "Province", Prop = "Province", Mapping = "PhysicalAddress.Province", ParentId = 22, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 31, Name = "Postal Code", Prop = "PostalCode", Mapping = "PhysicalAddress.PostalCode", ParentId = 22, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 32, Name = "Postal Address", Prop = "PostalAddress", Mapping = "PostalAddress", ParentId = null, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 33, Name = "Unit Number", Prop = "UnitNumber", Mapping = "PostalAddress.UnitNumber", ParentId = 32, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 34, Name = "Complex Name", Prop = "ComplexName", Mapping = "PostalAddress.ComplexName", ParentId = 32, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 35, Name = "Street Name", Prop = "StreetName", Mapping = "PostalAddress.StreetName", ParentId = 32, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 36, Name = "Street Number", Prop = "StreetNumber", Mapping = "PostalAddress.StreetNumber", ParentId = 32, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 37, Name = "Suburb Or District", Prop = "SuburbOrDistrict", Mapping = "PostalAddress.SuburbOrDistrict", ParentId = 32, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 38, Name = "City", Prop = "City", Mapping = "PostalAddress.City", ParentId = 32, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 39, Name = "Country", Prop = "Country", Mapping = "PostalAddress.Country", ParentId = 32, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 40, Name = "Province", Prop = "Province", Mapping = "PostalAddress.Province", ParentId = 32, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 41, Name = "Postal Code", Prop = "PostalCode", Mapping = "PostalAddress.PostalCode", ParentId = 32, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 51, Name = "People Champion", Prop = "PeopleChampion", Mapping = "ChampionEmployee.Name", ParentId = 32, Status = ItemStatus.Active
            },
            new DataReportColumnMenu
            {
                Id = 42, Name = null, Prop = null, Mapping = null, ParentId = null, Status = ItemStatus.Active, FieldCodeId = 1
            },
            new DataReportColumnMenu
            {
                Id = 43, Name = null, Prop = null, Mapping = null, ParentId = null, Status = ItemStatus.Active, FieldCodeId = 2
            },
            new DataReportColumnMenu
            {
                Id = 44, Name = null, Prop = null, Mapping = null, ParentId = null, Status = ItemStatus.Active, FieldCodeId = 3
            },
            new DataReportColumnMenu
            {
                Id = 45, Name = null, Prop = null, Mapping = null, ParentId = null, Status = ItemStatus.Active, FieldCodeId = 4
            },
            new DataReportColumnMenu
            {
                Id = 46, Name = null, Prop = null, Mapping = null, ParentId = null, Status = ItemStatus.Active, FieldCodeId = 5
            },
            new DataReportColumnMenu
            {
                Id = 47, Name = null, Prop = null, Mapping = null, ParentId = null, Status = ItemStatus.Active, FieldCodeId = 7
            },
            new DataReportColumnMenu
            {
                Id = 48, Name = null, Prop = null, Mapping = null, ParentId = null, Status = ItemStatus.Active, FieldCodeId = 8
            },
            new DataReportColumnMenu
            {
                Id = 49, Name = null, Prop = null, Mapping = null, ParentId = null, Status = ItemStatus.Active, FieldCodeId = 9
            },
            new DataReportColumnMenu
            {
                Id = 50, Name = null, Prop = null, Mapping = null, ParentId = null, Status = ItemStatus.Active, FieldCodeId = 10
            }
        );
    }

    public List<string> GetColumnNames(string tableName)
    {
        var dbSetProperty = GetType().GetProperties()
            .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                                 p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                                 p.PropertyType.GenericTypeArguments[0].Name == tableName);

        if (dbSetProperty == null)
        {
            throw new ArgumentException($"Table '{tableName}' not found in the DbContext.");
        }

        var entityType = Model.FindEntityType(dbSetProperty.PropertyType.GenericTypeArguments[0]);
        var columns = entityType.GetProperties().Select(p => p.GetColumnName()).ToList();
        return columns;
    }
}
