using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using ScooterRentalApp.Data;

namespace ScooterRentalApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<Client>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<Scooter>()
                .HasDiscriminator(b => b.Type)
                .HasValue<ManualScooter>(1)
                .HasValue<ElectricScooter>(2);
        }



        public DbSet<Client> Clients { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Scooter> Scooters { get; set; }

    }
}
