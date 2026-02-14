using BankAPI.Enum;

namespace BankAPI.DTO.AccountDTO;

public class AccountResponseDto
{
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; }
    public AccountType AccountType { get; set; }
    public int ClientId { get; set; }
    public string? AccountNumber {get; set;}
    public DateTime CreatedAt { get; set; }
    public AccountPlan Plan { get; set; }
}