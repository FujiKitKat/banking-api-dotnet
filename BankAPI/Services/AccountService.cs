using BankAPI.DTO.AccountDTO;
using BankAPI.Models;
using BankAPI.Repositories.Interfaces;
using BankAPI.Services.Interfaces;
using BankAPI.Enum;

namespace BankAPI.Services;
public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountResponseDto?> GetAccountByIdAsync(int id)
    {
        var account = await _accountRepository.GetAccountAsync(id);

        if (account == null)
        {
            return null;
        }
        
        var response = new AccountResponseDto
        {
            Balance = account.Balance,
            Status = account.Status,
            CreatedAt = account.CreatedAt,
            AccountNumber = account.AccountNumber,
            ClientId = account.ClientId,
        };
        
        return response;
    }

    public async Task<AccountResponseDto> CreateAccount(AccountCreateDto  accountCreateDto)
    {
        AccountModel account = new AccountModel
        {
            Balance = 0,
            CreatedAt = DateTime.UtcNow,
            Status = AccountStatus.Active,
            AccountNumber = Guid.NewGuid().ToString(),
            ClientId = accountCreateDto.ClientId,
            AccountType = accountCreateDto.AccountType
            
        };

        var createdAccount = await _accountRepository.CreateAccountAsync(account);
        
        var response = new AccountResponseDto
        {
            ClientId = createdAccount.ClientId,
            Balance = createdAccount.Balance,
            Status = createdAccount.Status,
            AccountType = createdAccount.AccountType
        };

        return response;
    }

    public async Task<List<AccountResponseDto>> GetAllAccountsByClientIdAsync(int clientId)
    {
        var accounts = await _accountRepository.GetAllAccountsByClientIdAsync(clientId);

        var response = new List<AccountResponseDto>();
        
        foreach (var account in accounts)
        {
            var dto = new AccountResponseDto
            {
                AccountNumber = account.AccountNumber,
                Balance = account.Balance,
                ClientId = account.ClientId,
                AccountType = account.AccountType,
                Status = account.Status
            };
            
            response.Add(dto);
        }
        
        return response;
    }

    public async Task<AccountResponseDto?> AccountUpdateStatusAsync(int id, AccountUpdateDto accountUpdateDto)
    {
        var account = await _accountRepository.GetAccountAsync(id);

        if (account == null)
        {
            return null;
        }

        if (account.Status == AccountStatus.Closed)
        {
            return null;
        }
        
        account.Status = accountUpdateDto.Status;

        await _accountRepository.SaveAsync();

        var response = new AccountResponseDto
        {
            ClientId = account.ClientId,
            Balance = account.Balance,
            AccountNumber = account.AccountNumber,
            Status = account.Status,
        };
        
        return response;
    }
}