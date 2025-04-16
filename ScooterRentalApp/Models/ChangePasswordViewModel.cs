using System.ComponentModel.DataAnnotations;

namespace ScooterRentalApp.Models
{
    public class SetPasswordViewModel
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Pole wymagane")]
        [Compare("NewPassword", ErrorMessage = "Wprowadzone hasła nie są takie same")]
        public string ConfirmPassword { get; set; }
    }
}
