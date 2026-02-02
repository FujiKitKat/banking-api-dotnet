using FluentValidation;
using BankAPI.DTO.AccountDTO;

namespace BankAPI.Validators.AccountValidators;

public class AccountCreateValidators : AbstractValidator<AccountCreateDto>
{
    public AccountCreateValidators()
    {
        RuleFor(x => x.ClientId)
            .NotEmpty()
            .WithMessage("ClientId is required");
        
        RuleFor(x => x.AccountType)
            .IsInEnum()
            .WithMessage("Account type is invalid");
    }
}