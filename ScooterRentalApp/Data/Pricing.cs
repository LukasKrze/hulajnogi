using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Data
{
    public class Pricing
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Cennik od")]
        public DateTime From { get; set; }

        [Display(Name = "Cennik do")]
        public DateTime? To { get; set; }

        [Required]
        [Display(Name = "Cena za godzinę [zł]")]
        public decimal PricePerUnit { get; set; }

        public Scooter Scooter { get; set; }

        public ICollection<Rental> Rentals { get; set; }
    }
}
