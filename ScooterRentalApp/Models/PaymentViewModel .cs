using ScooterRentalApp.Data;
using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Models
{
    public class PaymentViewModel
    {
        public int RentalId { get; set; }

        public string Model { get; set; }

        [Display(Name = "Numer seryjny")]
        public string SerialNumber { get; set; }

        [Display(Name = "Koszt za godzinę [zł]")]
        public decimal? Price { get; set; }


        [Display(Name = "Data wypożyczenia")]
        public DateTime From { get; set; }


        [Display(Name = "Planowana data zwrotu")]
        public DateTime? PlannedTo { get; set; }

        [Display(Name = "Data zwrotu")]
        public DateTime? To { get; set; }

        [Display(Name = "Liczba godzin do opłacenia")]
        public decimal HoursToPay { get; set; }

        [Display(Name = "Koszt do zapłaty")]
        public decimal Cost { get; set; }

        [Display(Name = "Zapłacono do tej pory")]
        public decimal RecentCost { get; set; }


        public bool Complaint { get; set; }


        public void CalculateCost()
        {
            DateTime to = To ?? PlannedTo.Value;
            HoursToPay = (decimal)double.Round((to - From).TotalHours, 2);
            Cost = Math.Max(HoursToPay * Price.Value - RecentCost, 0);
        }
    }
}
