using ScooterRentalApp.Data;
using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Models
{
    public class DemandingCustomerViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }

        public int NumberOfMessages { get; set; }

    }
}
