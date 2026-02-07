using BankAPI.DTO.AccountDTO;

namespace BankAPI.Services.Interfaces;

public interface IAccountService
{
    Task<AccountResponseDto> CreateAccount(AccountCreateDto accountCreateDto);
    Task<AccountResponseDto?> GetAccountByIdAsync(int id);
    Task<List<AccountResponseDto>> GetAllAccountsByClientIdAsync(int clientId);
    Task<AccountResponseDto?> AccountUpdateStatusAsync(int id, AccountUpdateDto accountUpdateDto);
    Task<bool> CloseAccountAsync(int id);
}