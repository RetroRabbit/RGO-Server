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

        public DbSet<UserGroup> usergroups {  get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Option> options { get; set; }
        public DbSet<FormSubmit> formsubmits { get; set; }
        public DbSet<Form> forms { get; set; }
        public DbSet<Field> fields { get; set; }
        public DbSet<Input> input { get; set; }
        public DbSet<Events> events { get; set; }
        public DbSet<Stacks> stacks { get; set; }
        public DbSet<UserStacks> userStacks { get; set; }
        public DbSet<Workshop> workshop { get; set; }
        public DbSet<Social> social { get; set; }
        public DbSet<Skill> skill { get; set; }
        public DbSet<Certifications> certifications { get; set; }
        public DbSet<Projects> projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>().HasKey(userGroup => userGroup.Id);
            modelBuilder.Entity<User>().HasKey(user => user.Id);
            modelBuilder.Entity<Option>().HasKey(option => option.Id);
            modelBuilder.Entity<FormSubmit>().HasKey(formSubmit => formSubmit.Id);
            modelBuilder.Entity<Form>().HasKey(form => form.Id);
            modelBuilder.Entity<Field>().HasKey(field => field.Id);
            modelBuilder.Entity<Input>().HasKey(input => input.Id);
            modelBuilder.Entity<Events>().HasKey(events => events.Id);
            modelBuilder.Entity<Stacks>().HasKey(stacks => stacks.Id);
            modelBuilder.Entity<UserStacks>().HasKey(userStacks => userStacks.Id);
            modelBuilder.Entity<Workshop>().HasKey(workshop => workshop.Id);
            modelBuilder.Entity<Social>().HasKey(social => social.Id);
            modelBuilder.Entity<Skill>().HasKey(skill => skill.Id);
            modelBuilder.Entity<Certifications>().HasKey(cert => cert.Id);
            modelBuilder.Entity<Projects>().HasKey(projects => projects.Id);
        }
    }
}
