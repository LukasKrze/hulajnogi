using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Data
{
    public class Pricing
    {
        public int Id { get; set; }

        [Required]
        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        [Required]
        public decimal PricePerUnit { get; set; }

        public Scooter Scooter { get; set; }

        public ICollection<Rental> Rentals { get; set; }
    }
}
