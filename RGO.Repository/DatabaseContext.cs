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
        public DbSet<Option> options { get; set; }
        public DbSet<UserGroup> usergroups {  get; set; }
        public DbSet<Events> events { get; set; }
        public DbSet<Stacks> stacks { get; set; }
        public DbSet<UserStacks> userStacks { get; set; }
        public DbSet<Workshop> workshop { get; set; }
        public DbSet<Form> forms { get; set; }
        public DbSet<Field> fields { get; set; } // TODO: Rename to formFields
        // TODO: DbSet<FormFieldOptions> formFieldOptions { get; set; }
        // TODO: DbSet<FormFieldDocuments> formFieldDocuments { get; set; }
        public DbSet<FormSubmit> formsubmits { get; set; }
        public DbSet<Input> input { get; set; } // TODO: Rename to formSubmitInput
        // TODO: DbSet<FormSubmitDocuments> formSubmitDocumets { get; set; }
        public DbSet<Social> social { get; set; }
        public DbSet<Skill> skill { get; set; }
        public DbSet<Certifications> certifications { get; set; }
        public DbSet<Projects> projects { get; set; }
    }
}
