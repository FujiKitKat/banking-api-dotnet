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

    public async Task<AccountResponseDto?> GetAccountByIdAsync(int id)
    {
        _logger.LogInformation(
            "Getting account {AccountId}", 
            id
            );
        
        var account = await _accountRepository.GetAccountAsync(id);

        if (account == null)
        {
            _logger.LogWarning(
                "Account{AccountId} not found",
                id
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
        };
        
        _logger.LogInformation(
            "Account {AccountId} retrieved", 
            id
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
        };

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

    public async Task<AccountResponseDto?> AccountUpdateStatusAsync(int id, AccountUpdateDto accountUpdateDto)
    {
        _logger.LogInformation(
            "Updating account {AccountId} status {Status}", 
            id, 
            accountUpdateDto.Status
            );
        
        var account = await _accountRepository.GetAccountAsync(id);

        if (account == null)
        {
            _logger.LogWarning(
                "Account{AccountId} not found", 
                id
                );
            
            return null;
        }

        if (account.Status == AccountStatus.Closed)
        {
            _logger.LogWarning(
                "Account {AccountId} status is closed", 
                id
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
            id, 
            oldStatus, 
            response.Status
            );
        
        return response;
    }

    public async Task<bool> CloseAccountAsync(int id)
    {
        _logger.LogInformation(
            "Closing Account {AccountId}", 
            id
            );
        
        var account = await _accountRepository.GetAccountAsync(id);

        if (account == null)
        {
            _logger.LogWarning(
                "Account {AccountId} was not found", 
                id
                );
            
            return false;
        }
        
        account.Status = AccountStatus.Closed;
        await _accountRepository.SaveAsync();
        
        _logger.LogInformation(
            "Account {AccountId} closed", 
            id
            );
        
        return true;
    }

    public async Task<AccountResponseDto?> AccountUpdatePlanAsync(int id, AccountUpdateDto accountUpdateDto)
    {
        _logger.LogInformation(
            "Updating plan of account {AccountId}", 
            id
            );
        
        var account = await _accountRepository.GetAccountAsync(id);

        if (account == null)
        {
            _logger.LogWarning(
                "Account {AccountId} was not found", 
                id
                );
            
            return null;
        }

        if (account.Status == AccountStatus.Closed)
        {
            _logger.LogWarning(
                "Account {AccountId} status is closed", 
                id
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
            id, 
            oldPlan, 
            response.Plan
            );
        
        return response;
    }
}