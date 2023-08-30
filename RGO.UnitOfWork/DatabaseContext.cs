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
            modelBuilder.Entity<EmployeeType>().HasData(ModelStateManage.EmployeeTypeSet());
            modelBuilder.Entity<FieldCode>().HasData(ModelStateManage.FieldCodeSet());
            modelBuilder.Entity<FieldCodeOptions>().HasData(ModelStateManage.FieldCodeOptionSet());
        }

        public DbSet<Employee> employees { get; set; }
        public DbSet<EmployeeRole> employeeRoles { get; set; }
        public DbSet<EmployeeDocument> employeeDocuments { get; set; }
        public DbSet<EmployeeProject> employeeProjects { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<RoleAccess> roleAccess { get; set; }
        public DbSet<OnboardingDocument> onboarding { get; set; }
        public DbSet<AuditLog> auditLogs { get; set; }
        public DbSet<Chart> Chart { get; set; }
        public DbSet<ChartRoleLink> ChartRoleLink { get; set; }
        public DbSet<FieldCode> fieldCodes { get; set; }
        public DbSet<FieldCodeOptions> fieldCodesOptions { get; set; }
        public DbSet<EmployeeData> employeeData { get; set; }
        public DbSet<PropertyAccess> propertyAccesses { get; set; }
        public DbSet<MetaProperty> metaProperties { get; set; }
        public DbSet<MetaPropertyOptions> metaPropertyOptions { get; set; }
        public DbSet<RoleAccessLink> roleAccessLinks { get; set; }
    }
}
