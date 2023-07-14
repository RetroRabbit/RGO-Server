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
        public DbSet<input> input { get; set; }
        public DbSet<events> events { get; set; }
        public DbSet<stacks> stacks { get; set; }
        public DbSet<userStacks> userStacks { get; set; }
        public DbSet<Workshop> workshop { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>().HasKey(usergroup => usergroup.id);
            modelBuilder.Entity<UserGroup>().Property(usergroup => usergroup.title).IsRequired();
        }

    }
}
