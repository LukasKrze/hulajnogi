using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Data
{

    public class Payment
    {

        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        
        public Rental Rental { get; set; }
    }
}
