using BankAPI.Enum;

namespace BankAPI.DTO.AccountDTO;

public class AccountUpdateDto
{
    public AccountStatus Status { get; set; }
    public AccountType AccountType { get; set; }
}