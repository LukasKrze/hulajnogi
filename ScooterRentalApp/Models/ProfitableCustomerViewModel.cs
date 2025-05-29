using ScooterRentalApp.Data;
using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Models
{
    public class ProfitableCustomerViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }

        public decimal TotalIncome { get; set; }

    }
}
