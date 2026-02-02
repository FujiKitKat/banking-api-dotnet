using BankAPI.DTO.ClientDTO;
using FluentValidation;

namespace BankAPI.Validators.ClientValidators;

public class ClientsUpdateValidator : AbstractValidator<ClientUpdateDTO>
{
    //Validator for client create DTO
    public ClientsUpdateValidator()
    {
        //Rule for name
        RuleFor(client => client.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MinimumLength(1)
            .MaximumLength(50);
        
        //Rule for Email adress
        RuleFor(client => client.Email)
            .NotEmpty()
            .WithMessage("Mail is required");
        
        //Rule for Phone number
        RuleFor(client => client.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^\+\d{10,15}$");;
    }
}