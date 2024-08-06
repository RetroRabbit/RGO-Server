using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.ATS;
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
