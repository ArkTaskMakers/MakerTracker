using Microsoft.EntityFrameworkCore;

namespace MakerTracker.DBModels
{
    public class MakerTrackerContext : DbContext
    {
        public MakerTrackerContext(DbContextOptions<MakerTrackerContext> options) : base(options)
        {
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Maker> Makers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<MakerOrder> MakerOrders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>().HasMany(x => x.TransactionFrom).WithOne(x => x.From);
            modelBuilder.Entity<Profile>().HasMany(x => x.TransactionTo).WithOne(x => x.To);
            modelBuilder.Entity<Product>().HasMany(x => x.PrecursorsRequired).WithOne(x => x.Parent);
            modelBuilder.Entity<Product>().HasMany(x => x.UsedInProducts).WithOne(x => x.Child);
            base.OnModelCreating(modelBuilder);
        }
    }
}