using Microsoft.EntityFrameworkCore;
using MakerTracker.DBModels;

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
        public DbSet<Material> Materials { get; set; }
        public DbSet<Equipment> Equipments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


        public DbSet<MakerTracker.DBModels.MakerOrder> MakerOrder { get; set; }
    }
     

}