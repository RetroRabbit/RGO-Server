using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RGO.UnitOfWork.Entities;

namespace RGO.UnitOfWork
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EmployeeType>().HasData(TestData.EmployeeTypeSet());
            modelBuilder.Entity<Employee>().HasData(TestData.EmployeeSet());
            modelBuilder.Entity<Role>().HasData(TestData.RoleSet());
            modelBuilder.Entity<RoleAccess>().HasData(TestData.RoleAccessSet());
            modelBuilder.Entity<RoleAccessLink>().HasData(TestData.RoleAccessLinkSet());
            modelBuilder.Entity<EmployeeRole>().HasData(TestData.EmployeeRole());
            modelBuilder.Entity<FieldCode>().HasData(TestData.FieldCodeSet());
            modelBuilder.Entity<FieldCodeOptions>().HasData(TestData.FieldCodeOptionSet());
            modelBuilder.Entity<PropertyAccess>().HasData(TestData.PropertyAccessSet());
            modelBuilder.Entity<EmployeeData>().HasData(TestData.EmployeeDataSet());
            modelBuilder.Entity<EmployeeEvaluation>().HasData(TestData.EmployeeEvaluationSet());
            modelBuilder.Entity<EmployeeEvaluationAudience>().HasData(TestData.EmployeeEvaluationAudienceSet());
            modelBuilder.Entity<EmployeeEvaluationRating>().HasData(TestData.EmployeeEvaluationRatingSet());
            modelBuilder.Entity<EmployeeEvaluationTemplate>().HasData(TestData.EmployeeEvaluationTemplateSet());
            modelBuilder.Entity<EmployeeEvaluationTemplateItem>().HasData(TestData.EmployeeEvaluationTemplateItemSet());
            modelBuilder.Entity<Client>().HasData(TestData.ClientList());
            modelBuilder.Entity<EmployeeAddress>().HasData(TestData.EmployeeAddressSet());
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
        public DbSet<OnboardingDocument> onboarding { get; set; }
        public DbSet<AuditLog> auditLogs { get; set; }
        public DbSet<Chart> Chart { get; set; }
        public DbSet<ChartRoleLink> ChartRoleLink { get; set; }
        public DbSet<FieldCode> fieldCodes { get; set; }
        public DbSet<FieldCodeOptions> fieldCodesOptions { get; set; }
        public DbSet<EmployeeData> employeeData { get; set; }
        public DbSet<EmployeeDate> employeeDate { get; set; }
        public DbSet<PropertyAccess> propertyAccesses { get; set; }
        public DbSet<RoleAccessLink> roleAccessLinks { get; set; }
        public DbSet<EmployeeBanking> employeeBanking { get; set;}
        public DbSet<Client> clients { get; set; }
    }
}
