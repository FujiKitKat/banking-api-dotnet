using BankAPI.DTO.AccountDTO;
using BankAPI.Models;
using BankAPI.Repositories.Interfaces;
using BankAPI.Services.Interfaces;
using BankAPI.Enum;

namespace BankAPI.Services;
public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<AccountService> _logger;
    public AccountService(IAccountRepository accountRepository, 
        ILogger<AccountService> logger)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }

    public async Task<AccountResponseDto?> GetAccountByIdAsync(
        int accountId, 
        int clientId
        )
    {
        _logger.LogInformation(
            "Getting account {AccountId}", 
            accountId
            );
        
        var account = await _accountRepository.GetAccountAsync(accountId,  clientId);

        if (account == null)
        {
            _logger.LogWarning(
                "Account{AccountId} not found",
                accountId
                );
            
            return null;
        }
        
        var response = new AccountResponseDto
        {
            Balance = account.Balance,
            Status = account.Status,
            CreatedAt = account.CreatedAt,
            AccountNumber = account.AccountNumber,
            ClientId = account.ClientId,
            AccountId = account.Id
        };
        
        _logger.LogInformation(
            "Account {AccountId} retrieved", 
            accountId
            );
        
        return response;
    }

    public async Task<AccountResponseDto> CreateAccount(AccountCreateDto  accountCreateDto)
    {
        _logger.LogInformation(
            "Creating account"
            );
        
        AccountModel account = new AccountModel
        {
            Balance = 0,
            CreatedAt = DateTime.UtcNow,
            Status = AccountStatus.Active,
            AccountNumber = Guid.NewGuid().ToString(),
            ClientId = accountCreateDto.ClientId,
            AccountType = accountCreateDto.AccountType,
            
        };

        var createdAccount = await _accountRepository.CreateAccountAsync(account);
        
        var response = new AccountResponseDto
        {
            ClientId = createdAccount.ClientId,
            Balance = createdAccount.Balance,
            Status = createdAccount.Status,
            AccountType = createdAccount.AccountType,
            AccountNumber = createdAccount.AccountNumber,
            AccountId = createdAccount.Id
        };

        await _accountRepository.SaveAsync();
        
        _logger.LogInformation(
            "Account was created successfully"
            );
        
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
        
        _logger.LogInformation(
            "Got {accountCount} accounts from the database",
            response.Count
            );
        
        return response;
    }

    public async Task<AccountResponseDto?> AccountUpdateStatusAsync(
        int accountId, 
        int clientId, 
        AccountUpdateDto accountUpdateDto
        )
    {
        _logger.LogInformation(
            "Updating account {AccountId} status {Status}", 
            accountId, 
            accountUpdateDto.Status
            );
        
        var account = await _accountRepository.GetAccountAsync(accountId, clientId);

        if (account == null)
        {
            _logger.LogWarning(
                "Account{AccountId} not found", 
                accountId
                );
            
            return null;
        }

        if (account.Status == AccountStatus.Closed)
        {
            _logger.LogWarning(
                "Account {AccountId} status is closed", 
                accountId
                );
            
            return null;
        }

        var oldStatus = account.Status;
        account.Status = accountUpdateDto.Status;

        await _accountRepository.SaveAsync();

        var response = new AccountResponseDto
        {
            ClientId = account.ClientId,
            Balance = account.Balance,
            AccountNumber = account.AccountNumber,
            Status = account.Status,
        };
        
        _logger.LogInformation(
            "Account {AccountId} status was updated from {OldStatus} to {NewStatus}", 
            accountId, 
            oldStatus, 
            response.Status
            );
        
        return response;
    }

    public async Task<bool> CloseAccountAsync(
        int accountId, 
        int clientId
        )
    {
        _logger.LogInformation(
            "Closing Account {AccountId}", 
            accountId
            );
        
        var account = await _accountRepository.GetAccountAsync(accountId, clientId);

        if (account == null)
        {
            _logger.LogWarning(
                "Account {AccountId} was not found", 
                accountId
                );
            
            return false;
        }
        
        account.Status = AccountStatus.Closed;
        await _accountRepository.SaveAsync();
        
        _logger.LogInformation(
            "Account {AccountId} closed", 
            accountId
            );
        
        return true;
    }

    public async Task<AccountResponseDto?> AccountUpdatePlanAsync(
        int accountId, 
        int clientId, 
        AccountUpdateDto accountUpdateDto
        )
    {
        _logger.LogInformation(
            "Updating plan of account {AccountId}", 
            accountId
            );
        
        var account = await _accountRepository.GetAccountAsync(accountId,  clientId);

        if (account == null)
        {
            _logger.LogWarning(
                "Account {AccountId} was not found", 
                accountId
                );
            
            return null;
        }

        if (account.Status == AccountStatus.Closed)
        {
            _logger.LogWarning(
                "Account {AccountId} status is closed", 
                accountId
                );
            
            return null;
        }
        
        var oldPlan = account.Plan;
        account.Plan =  accountUpdateDto.Plan;
        await _accountRepository.SaveAsync();

        var response = new AccountResponseDto
        {
            ClientId = account.ClientId,
            Balance = account.Balance,
            AccountNumber = account.AccountNumber,
            Status = account.Status,
            Plan = account.Plan
        };
        
        _logger.LogInformation(
            "Changed account{AccountId} plan from {OldPlan} to {NewPlan}", 
            accountId, 
            oldPlan, 
            response.Plan
            );
        
        return response;
    }
}