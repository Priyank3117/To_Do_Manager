using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Entities.Data
{
    public class ToDoManagerDBContext: DbContext
    {
        public ToDoManagerDBContext(DbContextOptions<ToDoManagerDBContext> dbContextOptions): base(dbContextOptions)
        {

        }

        public DbSet<Documents>   Documents { get; set; }
        public DbSet<Users> Users { get; set; }

        public DbSet<ResetPassword> ResetPassword { get; set; }

        public DbSet<Teams> Teams { get; set; }

        public DbSet<TeamMembers> TeamMembers { get; set; }

        public DbSet<Tasks> Tasks { get; set; }

        public DbSet<InvitedUsers> InvitedUsers { get; set; }

        public DbSet<Notifications> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TeamMembers>()
                .Property(U => U.Status)
                .HasConversion<string>()
                .HasMaxLength(50);
            modelBuilder.Entity<TeamMembers>()
                .Property(U => U.Role)
                .HasConversion<string>()
                .HasMaxLength(50);
            modelBuilder.Entity<Tasks>()
                .Property(U => U.AssignedBy)
                .HasConversion<string>()
                .HasMaxLength(50);
            modelBuilder.Entity<Users>()
                .Property(U => U.Gender)
                .HasConversion<string>()
                .HasMaxLength(10);
            modelBuilder.Entity<Notifications>()
                .Property(U => U.Type)
                .HasConversion<string>()
                .HasMaxLength(50);
            //modelBuilder.Entity<TeamMembers>().ToTable(tb => tb.HasTrigger("TriggerName"));
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}
