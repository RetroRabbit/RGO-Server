using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Entities.ATS;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using RR.UnitOfWork.Entities;

namespace RR.UnitOfWork;

public interface IDatabaseContext
{
    DbSet<Employee> employees { get; set; }
    DbSet<EmployeeRole> employeeRoles { get; set; }
    DbSet<EmployeeAddress> employeeAddresses { get; set; }
    DbSet<EmployeeDocument> employeeDocuments { get; set; }
    DbSet<EmployeeProject> employeeProjects { get; set; }
    DbSet<EmployeeEvaluation> employeeEvaluations { get; set; }
    DbSet<EmployeeEvaluationAudience> employeeEvaluationAudiences { get; set; }
    DbSet<EmployeeEvaluationRating> employeeEvaluationRatings { get; set; }
    DbSet<EmployeeEvaluationTemplate> employeeEvaluationTemplates { get; set; }
    DbSet<EmployeeEvaluationTemplateItem> employeeEvaluationTemplateItem { get; set; }
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
    DbSet<MonthlyEmployeeTotal> monthlyEmployeeTotal { get; set; }
    DbSet<Applicant> applicant { get; set; }
    DbSet<ErrorLogging> errorLogging { get; set; }
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
    public DbSet<EmployeeDocument> employeeDocuments { get; set; }
    public DbSet<EmployeeProject> employeeProjects { get; set; }
    public DbSet<EmployeeEvaluation> employeeEvaluations { get; set; }
    public DbSet<EmployeeEvaluationAudience> employeeEvaluationAudiences { get; set; }
    public DbSet<EmployeeEvaluationRating> employeeEvaluationRatings { get; set; }
    public DbSet<EmployeeEvaluationTemplate> employeeEvaluationTemplates { get; set; }
    public DbSet<EmployeeEvaluationTemplateItem> employeeEvaluationTemplateItem { get; set; }
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
    public DbSet<MonthlyEmployeeTotal> monthlyEmployeeTotal { get; set; }
    public DbSet<Applicant> applicant { get; set; }
    public DbSet<ErrorLogging> errorLogging { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("appsettings.json")
               .AddUserSecrets<DatabaseContext>();
        var configuration = builder.Build();
        optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__Default"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<EmployeeType>().HasData(TestData.EmployeeTypeSet());
        modelBuilder.Entity<Client>().HasData(TestData.ClientList());
        modelBuilder.Entity<Role>().HasData(TestData.RoleSet());
        modelBuilder.Entity<RoleAccess>().HasData(TestData.RoleAccessSet());
        modelBuilder.Entity<RoleAccessLink>().HasData(TestData.RoleAccessLinkSet());
        modelBuilder.Entity<EmployeeRole>().HasData(TestData.EmployeeRole());
        modelBuilder.Entity<FieldCode>().HasData(TestData.FieldCodeSet());
        modelBuilder.Entity<FieldCodeOptions>().HasData(TestData.FieldCodeOptionSet());
        modelBuilder.Entity<PropertyAccess>().HasData(TestData.PropertyAccessSet());
        modelBuilder.Entity<EmployeeEvaluation>().HasData(TestData.EmployeeEvaluationSet());
        modelBuilder.Entity<EmployeeEvaluationAudience>().HasData(TestData.EmployeeEvaluationAudienceSet());
        modelBuilder.Entity<EmployeeEvaluationRating>().HasData(TestData.EmployeeEvaluationRatingSet());
        modelBuilder.Entity<EmployeeEvaluationTemplate>().HasData(TestData.EmployeeEvaluationTemplateSet());
        modelBuilder.Entity<EmployeeEvaluationTemplateItem>().HasData(TestData.EmployeeEvaluationTemplateItemSet());
        modelBuilder.Entity<EmployeeAddress>().HasData(TestData.EmployeeAddressSet());
        modelBuilder.Entity<Employee>().HasData(TestData.EmployeeSet());
        modelBuilder.Entity<EmployeeData>().HasData(TestData.EmployeeDataSet());
    }
}