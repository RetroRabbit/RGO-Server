using Microsoft.EntityFrameworkCore;
using RGO.Repository.Entities;

namespace RGO.Repository
{
    public class DatabaseContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=localhost;Database=RGO;Username=postgres;Password=postgrespw", b => b.MigrationsAssembly("RGO.App"));
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>().HasKey(userGroup => userGroup.id);
            modelBuilder.Entity<User>().HasKey(user => user.id);
            modelBuilder.Entity<Option>().HasKey(option => option.id);
            modelBuilder.Entity<FormSubmit>().HasKey(formSubmit => formSubmit.id);
            modelBuilder.Entity<Form>().HasKey(form => form.id);
            modelBuilder.Entity<Field>().HasKey(field => field.id);
        }
    }
}
