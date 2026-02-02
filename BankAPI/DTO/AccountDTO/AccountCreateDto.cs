using BankAPI.Enum;

namespace BankAPI.DTO.AccountDTO;

public class AccountCreateDto
{
    public int ClientId { get; set; }
    public AccountType AccountType { get; set; }
}