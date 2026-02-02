using BankAPI.DTO.AccountDTO;
using BankAPI.Enum;

namespace BankAPI.Services.Interfaces;

public interface IAccountService
{
    Task<AccountResponseDto> CreateAccount(AccountCreateDto accountCreateDto);
    Task<AccountResponseDto?> GetAccountByIdAsync(int id);
    Task<List<AccountResponseDto>> GetAllAccountsByClientIdAsync(int clientId);
    
    Task<AccountResponseDto?> AccountUpdateStatusAsync(int id, AccountUpdateDto accountUpdateDto);
}