using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RGO.Repository.Entities;

namespace RGO.Repository
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

        public DbSet<User> users { get; set; }
        public DbSet<GradGroup> gradGroups {  get; set; }
        public DbSet<GradEvents> events { get; set; }
        public DbSet<Stacks> stacks { get; set; }
        public DbSet<UserStacks> userStacks { get; set; }
        public DbSet<Workshop> workshop { get; set; }
        public DbSet<Social> social { get; set; }
        public DbSet<Skill> skill { get; set; }
        public DbSet<Certifications> certifications { get; set; }
        public DbSet<Projects> projects { get; set; }
    }
}
