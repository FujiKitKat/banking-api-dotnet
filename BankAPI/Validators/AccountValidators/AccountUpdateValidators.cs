using BankAPI.DTO.AccountDTO;
using FluentValidation;

namespace BankAPI.Validators.AccountValidators;

public class AccountUpdateValidators : AbstractValidator<AccountUpdateDto>
{
    public AccountUpdateValidators()
    {
        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status should be in in-enum");
        
        RuleFor(x => x.AccountType)
            .IsInEnum()
            .WithMessage("Account type should be in-enum");
        
        RuleFor(x => x.Plan)
            .IsInEnum()
            .WithMessage("Plan should be in-enum");
    }
}