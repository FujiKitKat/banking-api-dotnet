using BankAPI.Enum;

namespace BankAPI.Models.ClientModels;

public class ClientModel
{
    public int Id {get; set; }
    public string Name {get; set; } = string.Empty;
    public string Email {get; set; } = string.Empty;
    public string PhoneNumber {get; set; }
    public ClientStatus Status {get; set; }
    public DateTime CreateDate {get; set; }
    public ICollection<AccountModel> Accounts { get; set; }
}