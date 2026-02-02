using BankAPI.Models;

namespace BankAPI.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<AccountModel?> GetAccountAsync(int id);
    Task<List<AccountModel>> GetAllAccountsByClientIdAsync(int clientId);
    Task<AccountModel> CreateAccountAsync(AccountModel account);
    Task SaveAsync();
}