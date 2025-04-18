using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Data
{
    public class Rental
    {
        public int Id { get; set; }

        [Required]
        public DateTime RentalDate { get; set; }

        public DateTime? PlannedReturnDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public decimal Cost { get; set; }

     
        public Client Client { get; set; }

        
        public Scooter Scooter { get; set; }

        public ICollection<Payment> Payments { get; set; }

        public Pricing Pricing { get; set; }
    }
}
