using budgetManager.Data.Seeder;
using budgetManager.Models;
using Microsoft.EntityFrameworkCore;

namespace budgetManager.Data
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) :
            base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<User> Accounts { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<HtmlTemplate> HtmlTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new AccountConfiguration());
            builder.ApplyConfiguration(new SeatConfiguration());
            builder.ApplyConfiguration(new EntryConfiguration());

            builder
                .Entity<Entry>()
                .HasOne(e => e.Seat)
                .WithMany(s => s.Entries)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Entry>()
                .HasOne(e => e.Account)
                .WithMany(s => s.Entries)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder
                .Entity<Account>()
                .HasOne(e => e.User)
                .WithMany(s => s.Accounts)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
