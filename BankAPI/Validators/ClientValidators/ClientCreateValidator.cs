using BankAPI.DTO.ClientDTO;
using FluentValidation;

namespace BankAPI.Validators.ClientValidators;

public class ClientCreateValidator : AbstractValidator<ClientCreateDTO>
{
    public ClientCreateValidator()
    {
        RuleFor(x =>x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters.")
            .MinimumLength(1);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Address is required.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required")
            .Matches(@"^\+\d{10,15}$");
    }
}