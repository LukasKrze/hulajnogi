using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Data
{
    public abstract class Scooter
    {
        public int Id { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        public float MaxSpeed { get; set; }

        public float Range { get; set; }

        public int Type { get; set; } // 1 - electric, 2 - manual

        public bool HasKickstand { get; set; }
             

        public int? CurrentRentalId { get; set; } 

        public int YearOfProduction { get; set; }

        
        public ICollection<Rental> Rentals { get; set; }
        public ICollection<Pricing> Pricings { get; set; }
    }

}
