using BankAPI.DTO.AccountDTO;

namespace BankAPI.Services.Interfaces;

public interface IAccountService
{
    Task<AccountResponseDto> CreateAccount(AccountCreateDto accountCreateDto);
    Task<AccountResponseDto?> GetAccountByIdAsync(int accountId, int clientId);
    Task<List<AccountResponseDto>> GetAllAccountsByClientIdAsync(int clientId);
    Task<AccountResponseDto?> AccountUpdateStatusAsync(int accountId, int clientId, AccountUpdateDto accountUpdateDto);
    Task<AccountResponseDto?> AccountUpdatePlanAsync(int accountId, int clientId, AccountUpdateDto accountUpdateDto);
    Task<bool> CloseAccountAsync(int accountId, int clientId);
}