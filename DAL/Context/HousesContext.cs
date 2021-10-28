using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class HousesContext : DbContext
    {
        public DbSet<House> Houses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseCosmos(
                "https://localhost:8081",
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                "BuyMyHouseDB");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<House>()
                .ToContainer("Houses");

            modelBuilder.Entity<House>()
                .HasNoDiscriminator();

            modelBuilder.Entity<House>()
                .HasPartitionKey(h => h.ZipCode);

            modelBuilder.Entity<House>()
                .UseETagConcurrency();
        }
    }
}