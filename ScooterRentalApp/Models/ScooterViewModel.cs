using ScooterRentalApp.Data;
using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Models
{
    public class ScooterViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Kategoria")]
        public int CategoryId { get; set; }

        [Display(Name = "Kategoria")]
        public string? Category { get; set; }


        [Required(ErrorMessage = "{0} jest wymagany")]
        public string Model { get; set; }

        [Display(Name = "Numer seryjny")]
        [Required(ErrorMessage = "{0} jest wymagany")]
        public string SerialNumber { get; set; }

        [Display(Name = "Maksymalna prędkość [km/h]")]
        [Required(ErrorMessage = "{0} jest wymagana")]
        [Range(0, 100, ErrorMessage = "Prędkość hulajnogi musi być nieujemną liczbą rzeczywistą nie większą od 100")]
        public float MaxSpeed { get; set; }

        [Display(Name = "Zasięg [km]")]
        [Required(ErrorMessage = "{0} jest wymagany")]
        [Range(0, 50, ErrorMessage = "Zasięg hulajnogi musi być nieujemną liczbą rzeczywistą nie większą od 50")]
        public float Range { get; set; }

        [Display(Name = "Rodzaj hulajnogi")]
        [Required(ErrorMessage = "{0} jest wymagany")]
        [Range(1, 2, ErrorMessage = "Pole musi mieć wartość 1 lub 2")]
        public int Type { get; set; } // 1 - electric, 2 - manual

        [Display(Name = "Czy model posiada nóżkę?")]
        public bool HasKickstand { get; set; }


        [Display(Name = "Opis")]
        [Required(ErrorMessage = "{0} jest wymagany")]
        public string Description { get; set; }

        public string Picture { get; set; }

        public int? CurrentRentalId { get; set; }


        [Display(Name = "Rok produkcji")]
        [Required(ErrorMessage = "{0} jest wymagany")]
        [Range(1995, 2050, ErrorMessage = "Pole musi mieć wartość całkowią pomiędzy 1995 oraz 2050")]

        public int YearOfProduction { get; set; }

        [Display(Name = "Pojemność baterii")]
        public float? BatteryCapacity { get; set; }

        [Display(Name = "Rozmiar kółek")]
        public float? WheelSize { get; set; }

        [Display(Name = "Koszt za godzinę [zł]")]
        [RegularExpression("^\\d+(\\.\\d{1,2})?$", ErrorMessage = "Koszt musi być liczbą z maksymalnie dwoma miejscami po przecinku")]
        [Required(ErrorMessage = "Koszt jest wymagany")]
        [Range(0.01, 1000, ErrorMessage = "Koszt musi być wartością dodatnią mniejszą od 1000 zł")]
        public decimal? InitialPrice { get; set; }

        public List<Pricing>? Pricings { get; set; }

        public static ScooterViewModel MapToViewModel(Scooter scooter)
        {
            return new ScooterViewModel
            {
                BatteryCapacity = (scooter as ElectricScooter)?.BatteryCapacity ?? 0,
                CurrentRentalId = scooter.CurrentRentalId,
                HasKickstand = scooter.HasKickstand,
                Id = scooter.Id,
                MaxSpeed = scooter.MaxSpeed,
                Range = scooter.Range,
                Type = scooter.Type,
                Model = scooter.Model,
                SerialNumber = scooter.SerialNumber,
                WheelSize = (scooter as ManualScooter)?.WheelSize ?? 0,
                YearOfProduction = scooter.YearOfProduction,
                Pricings = scooter.Pricings?.ToList(),
                Category = scooter.Category.Name,
                CategoryId = scooter.Category.Id,
                Description = scooter.Description,
                Picture = scooter.Picture,
                InitialPrice = 1
            };

        }

        public Scooter MapToModel()
        {
            Scooter scooter = Type == 1
                ? new ManualScooter { WheelSize = WheelSize.Value }
                : new ElectricScooter { BatteryCapacity = BatteryCapacity.Value };
            scooter.Model = Model;
            scooter.CurrentRentalId = CurrentRentalId;
            scooter.HasKickstand = HasKickstand;
            scooter.Id = Id;
            scooter.MaxSpeed = MaxSpeed;
            scooter.Range = Range;
            scooter.Type = Type;
            scooter.Model = Model;
            scooter.SerialNumber = SerialNumber;
            scooter.YearOfProduction = YearOfProduction;
            scooter.Picture = Picture;
            scooter.Description = Description;
            return scooter;
        }

    }
}
