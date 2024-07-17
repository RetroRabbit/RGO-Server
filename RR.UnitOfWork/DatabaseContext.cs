using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.ATS;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.Shared;

namespace RR.UnitOfWork;

public class DatabaseContext : DbContext
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
    public DbSet<EmployeeQualification> employeeQualifications { get; set; }
    public DbSet<EmployeeSalaryDetails> employeeSalaryDetails { get; set; }
    public DbSet<Role> roles { get; set; }
    public DbSet<RoleAccess> roleAccess { get; set; }
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
    public DbSet<DataReportAccess> dataReportAccess { get; set; }
    public DbSet<EmailTemplate> emailTemplate { get; set; }
    public DbSet<EmailHistory> emailHistory { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(optionsBuilder.IsConfigured)
            return;
        optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__Default"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Description = "SuperAdmin", AuthRoleId = "" },
            new Role { Id = 2, Description = "Admin", AuthRoleId = "" },
            new Role { Id = 3, Description = "Employee", AuthRoleId = "" },
            new Role { Id = 4, Description = "Talent", AuthRoleId = "" },
            new Role { Id = 5, Description = "Journey", AuthRoleId = "" }
        );

        modelBuilder.Entity<RoleAccess>().HasData(
            new RoleAccess { Id = 1, Grouping = "Employee Data", Permission = "ViewEmployee" },
            new RoleAccess { Id = 2, Grouping = "Employee Data", Permission = "AddEmployee" },
            new RoleAccess { Id = 3, Grouping = "Employee Data", Permission = "EditEmployee" },
            new RoleAccess { Id = 4, Grouping = "Employee Data", Permission = "DeleteEmployee" },
            new RoleAccess { Id = 5, Grouping = "Charts", Permission = "ViewChart" },
            new RoleAccess { Id = 6, Grouping = "Charts", Permission = "AddChart" },
            new RoleAccess { Id = 7, Grouping = "Charts", Permission = "EditChart" },
            new RoleAccess { Id = 8, Grouping = "Charts", Permission = "DeleteChart" },
            new RoleAccess { Id = 9, Grouping = "Employee Data", Permission = "ViewOwnInfo" },
            new RoleAccess { Id = 10, Grouping = "Employee Data", Permission = "EditOwnInfo" }
        );

        modelBuilder.Entity<RoleAccessLink>().HasData(
            new RoleAccessLink { Id = 1, RoleAccessId = 1, RoleId = 1 },
            new RoleAccessLink { Id = 2, RoleAccessId = 2, RoleId = 1 },
            new RoleAccessLink { Id = 3, RoleAccessId = 3, RoleId = 1 },
            new RoleAccessLink { Id = 4, RoleAccessId = 4, RoleId = 1 },
            new RoleAccessLink { Id = 5, RoleAccessId = 5, RoleId = 1 },
            new RoleAccessLink { Id = 6, RoleAccessId = 6, RoleId = 1 },
            new RoleAccessLink { Id = 7, RoleAccessId = 7, RoleId = 1 },
            new RoleAccessLink { Id = 8, RoleAccessId = 8, RoleId = 1 },
            new RoleAccessLink { Id = 9, RoleAccessId = 9, RoleId = 1 },
            new RoleAccessLink { Id = 10, RoleAccessId = 10, RoleId = 1 },
            new RoleAccessLink { Id = 11, RoleAccessId = 1, RoleId = 2 },
            new RoleAccessLink { Id = 12, RoleAccessId = 2, RoleId = 2 },
            new RoleAccessLink { Id = 13, RoleAccessId = 3, RoleId = 2 },
            new RoleAccessLink { Id = 14, RoleAccessId = 4, RoleId = 2 },
            new RoleAccessLink { Id = 15, RoleAccessId = 5, RoleId = 2 },
            new RoleAccessLink { Id = 16, RoleAccessId = 6, RoleId = 2 },
            new RoleAccessLink { Id = 17, RoleAccessId = 7, RoleId = 2 },
            new RoleAccessLink { Id = 18, RoleAccessId = 8, RoleId = 2 },
            new RoleAccessLink { Id = 19, RoleAccessId = 9, RoleId = 2 },
            new RoleAccessLink { Id = 20, RoleAccessId = 10, RoleId = 2 },
            new RoleAccessLink { Id = 21, RoleAccessId = 1, RoleId = 3 },
            new RoleAccessLink { Id = 22, RoleAccessId = 3, RoleId = 3 },
            new RoleAccessLink { Id = 23, RoleAccessId = 9, RoleId = 3 },
            new RoleAccessLink { Id = 24, RoleAccessId = 10, RoleId = 3 },
            new RoleAccessLink { Id = 25, RoleAccessId = 5, RoleId = 4 },
            new RoleAccessLink { Id = 26, RoleAccessId = 6, RoleId = 4 },
            new RoleAccessLink { Id = 27, RoleAccessId = 7, RoleId = 4 },
            new RoleAccessLink { Id = 28, RoleAccessId = 8, RoleId = 4 },
            new RoleAccessLink { Id = 29, RoleAccessId = 9, RoleId = 4 },
            new RoleAccessLink { Id = 30, RoleAccessId = 10, RoleId = 4 },
            new RoleAccessLink { Id = 31, RoleAccessId = 5, RoleId = 5 },
            new RoleAccessLink { Id = 32, RoleAccessId = 6, RoleId = 5 },
            new RoleAccessLink { Id = 33, RoleAccessId = 7, RoleId = 5 },
            new RoleAccessLink { Id = 34, RoleAccessId = 8, RoleId = 5 },
            new RoleAccessLink { Id = 35, RoleAccessId = 9, RoleId = 5 },
            new RoleAccessLink { Id = 36, RoleAccessId = 10, RoleId = 5 }
        );

        modelBuilder.Entity<DataReportColumnMenu>().HasData(
            new DataReportColumnMenu { Id = 1, Name = "Employee Number", Prop = "EmployeeNumber", Mapping = "EmployeeNumber", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 2, Name = "Engagement Date", Prop = "EngagementDate", Mapping = "EngagementDate", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 3, Name = "Termination Date", Prop = "TerminationDate", Mapping = "TerminationDate", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 4, Name = "Level", Prop = "Level", Mapping = "Level", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 5, Name = "Name", Prop = "Name", Mapping = "Name", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 6, Name = "Initials", Prop = "Initials", Mapping = "Initials", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 7, Name = "Surname", Prop = "Surname", Mapping = "Surname", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 8, Name = "Date Of Birth", Prop = "DateOfBirth", Mapping = "DateOfBirth", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 9, Name = "Country Of Birth", Prop = "CountryOfBirth", Mapping = "CountryOfBirth", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 10, Name = "Nationality", Prop = "Nationality", Mapping = "Nationality", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 11, Name = "Race", Prop = "Race", Mapping = "Race", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 12, Name = "Gender", Prop = "Gender", Mapping = "Gender", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 13, Name = "Email", Prop = "Email", Mapping = "Email", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 14, Name = "Personal Email", Prop = "PersonalEmail", Mapping = "PersonalEmail", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 15, Name = "Cellphone No", Prop = "CellphoneNo", Mapping = "CellphoneNo", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 16, Name = "House No", Prop = "HouseNo", Mapping = "HouseNo", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 17, Name = "Emergency Contact Name", Prop = "EmergencyContactName", Mapping = "EmergencyContactName", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 18, Name = "Emergency Contact No", Prop = "EmergencyContactNo", Mapping = "EmergencyContactNo", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 19, Name = "Inactive Reason", Prop = "InactiveReason", Mapping = "InactiveReason", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 20, Name = "Role", Prop = "Role", Mapping = "EmployeeType.Name", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 21, Name = "Client Assigned", Prop = "ClientAssignedName", Mapping = "ClientAssigned.Name", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 22, Name = "Physical Address", Prop = "PhysicalAddress", Mapping = "PhysicalAddress", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 23, Name = "Unit Number", Prop = "PhysicalAddressUnitNumber", Mapping = "PhysicalAddress.UnitNumber", ParentId = 22, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 24, Name = "Complex Name", Prop = "PhysicalAddressComplexName", Mapping = "PhysicalAddress.ComplexName", ParentId = 22, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 25, Name = "Street Name", Prop = "PhysicalAddressStreetName", Mapping = "PhysicalAddress.StreetName", ParentId = 22, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 26, Name = "Street Number", Prop = "PhysicalAddressStreetNumber", Mapping = "PhysicalAddress.StreetNumber", ParentId = 22, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 27, Name = "Suburb Or District", Prop = "PhysicalAddressSuburbOrDistrict", Mapping = "PhysicalAddress.SuburbOrDistrict", ParentId = 22, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 28, Name = "City", Prop = "PhysicalAddressCity", Mapping = "PhysicalAddress.City", ParentId = 22, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 29, Name = "Country", Prop = "PhysicalAddressCountry", Mapping = "PhysicalAddress.Country", ParentId = 22, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 30, Name = "Province", Prop = "PhysicalAddressProvince", Mapping = "PhysicalAddress.Province", ParentId = 22, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 31, Name = "Postal Code", Prop = "PhysicalAddressPostalCode", Mapping = "PhysicalAddress.PostalCode", ParentId = 22, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 32, Name = "Postal Address", Prop = "PostalAddress", Mapping = "PostalAddress", ParentId = null, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 33, Name = "Unit Number", Prop = "PostalAddressUnitNumber", Mapping = "PostalAddress.UnitNumber", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 34, Name = "Complex Name", Prop = "PostalAddressComplexName", Mapping = "PostalAddress.ComplexName", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 35, Name = "Street Name", Prop = "PostalAddressStreetName", Mapping = "PostalAddress.StreetName", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 36, Name = "Street Number", Prop = "PostalAddressStreetNumber", Mapping = "PostalAddress.StreetNumber", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 37, Name = "Suburb Or District", Prop = "PostalAddressSuburbOrDistrict", Mapping = "PostalAddress.SuburbOrDistrict", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 38, Name = "City", Prop = "PostalAddressCity", Mapping = "PostalAddress.City", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 39, Name = "Country", Prop = "PostalAddressCountry", Mapping = "PostalAddress.Country", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 40, Name = "Province", Prop = "PostalAddressProvince", Mapping = "PostalAddress.Province", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 41, Name = "Postal Code", Prop = "PostalAddressPostalCode", Mapping = "PostalAddress.PostalCode", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 42, Name = "People Champion", Prop = "ChampionEmployeeName", Mapping = "ChampionEmployee.Name", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 43, Name = "Qualification", Prop = "EmployeeQualificationHighestQualification", Mapping = "EmployeeQualification.HighestQualification", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 44, Name = "School", Prop = "EmployeeQualificationSchool", Mapping = "EmployeeQualification.School", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 45, Name = "Field Of Study", Prop = "EmployeeQualificationFieldOfStudy", Mapping = "EmployeeQualification.FieldOfStudy", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 46, Name = "NQF Level", Prop = "EmployeeQualificationNQFLevel", Mapping = "EmployeeQualification.NQFLevel", ParentId = 32, Status = ItemStatus.Active },
            new DataReportColumnMenu { Id = 47, Name = "Qualification Year", Prop = "EmployeeQualificationYear", Mapping = "EmployeeQualification.Year", ParentId = 32, Status = ItemStatus.Active }
        );

        modelBuilder.Entity<DataReport>().HasData(new DataReport { Id = 1, Name = "Availability Snapshot", Code = "AS01", Status = ItemStatus.Active });

        var sequence = 0;
        modelBuilder.Entity<DataReportColumns>().HasData(
            new DataReportColumns { Id = 1, Status = ItemStatus.Active, ReportId = 1, MenuId = 5,    Sequence = sequence++, FieldType = DataReportColumnType.Employee },
            new DataReportColumns { Id = 2, Status = ItemStatus.Active, ReportId = 1, MenuId = 7,    Sequence = sequence++, FieldType = DataReportColumnType.Employee },
            new DataReportColumns { Id = 3, Status = ItemStatus.Active, ReportId = 1, MenuId = 20,   Sequence = sequence++, FieldType = DataReportColumnType.Employee },
            new DataReportColumns { Id = 4, Status = ItemStatus.Active, ReportId = 1, MenuId = 4,    Sequence = sequence++, FieldType = DataReportColumnType.Employee },
            new DataReportColumns { Id = 6, Status = ItemStatus.Active, ReportId = 1, MenuId = null, Sequence = sequence++, FieldType = DataReportColumnType.Text, CustomName = "Internal Project", CustomProp = "InternalProject" },
            new DataReportColumns { Id = 7, Status = ItemStatus.Active, ReportId = 1, MenuId = null, Sequence = sequence++, FieldType = DataReportColumnType.Text, CustomName = "Open Source", CustomProp = "OpenSource" },
            new DataReportColumns { Id = 8, Status = ItemStatus.Active, ReportId = 1, MenuId = null, Sequence = sequence++, FieldType = DataReportColumnType.Text, CustomName = "Notes", CustomProp = "Notes" }
        );

        modelBuilder.Entity<DataReportFilter>().HasData(
            new DataReportFilter { Id = 1, Status = ItemStatus.Active, ReportId = 1, Table = "Employee", Column = "clientAllocated", Condition = "IS NULL", Value = null, Select = "id", },
            new DataReportFilter { Id = 2, Status = ItemStatus.Active, ReportId = 1, Table = "Employee", Column = "employeeTypeId", Condition = "IN", Value = "(2,3,4)", Select = "id", }
        );

        modelBuilder.Entity<DataReportAccess>().HasData(
            new DataReportAccess { Id = 1, ReportId = 1, EmployeeId = null, RoleId = 1 }
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
