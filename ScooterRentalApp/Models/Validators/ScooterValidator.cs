using FluentValidation;

namespace ScooterRentalApp.Models.Validators
{
    public class ScooterValidator : AbstractValidator<ScooterViewModel>
    {
        public ScooterValidator()
        {
            RuleFor(s => s.BatteryCapacity).NotEmpty()
                .When(s => s.Type == 2)
                .WithMessage("Należy podać pojemność baterii dla hulajnogi elektrycznej");
            RuleFor(s => s.WheelSize).NotEmpty()
                .When(s => s.Type == 1)
                .WithMessage("Należy podać rozmiar koła dla hulajnogi manualnej");
            RuleFor(s => s.InitialPrice).NotEmpty()
                .When(s => s.Id == 0)
                .WithMessage("Należy podać kwotę, żeby klient mógł dokonać wypożyczenia");

        }
    }
}
