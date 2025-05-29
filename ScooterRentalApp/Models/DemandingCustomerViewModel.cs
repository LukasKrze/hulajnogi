using ScooterRentalApp.Data;
using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Models
{
    public class RentedScooterViewModel
    {
        public string Model { get; set; }

        public string SerialNumber { get; set; }
 
        public int NumberOfRentals { get; set; }

    }
}
