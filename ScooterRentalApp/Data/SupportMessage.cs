using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Data
{
    public class SupportMessage
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Wiadomość")]
        public string Message { get; set; } = string.Empty;

        [Display(Name = "Klient")]
        public Client Client { get; set; }

        [Display(Name = "Czy wysłana przez klienta?")]

        public bool FromClient { get; set; }

        [Display(Name = "Data utworzenia")]
        public DateTime Created { get; set; }

    }
}
