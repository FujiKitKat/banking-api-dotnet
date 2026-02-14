using BankAPI.Enum;
using BankAPI.Models.ClientModels;

namespace BankAPI.Models;

public class AccountModel
{
    public int Id { get; set; }
    public string? AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public AccountStatus Status { get; set; } 
    public AccountType AccountType { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ClientId { get; set; }
    public ClientModel Client { get; set; }
    public AccountPlan Plan { get; set; }
}