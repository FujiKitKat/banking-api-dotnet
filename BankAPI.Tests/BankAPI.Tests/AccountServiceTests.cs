using Moq;
using BankAPI.DTO.AccountDTO;
using BankAPI.Enum;
using BankAPI.Repositories.Interfaces;
using BankAPI.Models;
using BankAPI.Services;
using Microsoft.Extensions.Logging;
using FluentAssertions;

namespace BankAPI.Tests;

public class AccountServiceTests
{
    [Fact]
    public async Task GetAccountById_ShouldReturnAccount_WhenAccountExists()
    {
        var mockAccountRepository = new Mock<IAccountRepository>();
        var mockLogger = new Mock<ILogger<AccountService>>();

        var clientId = 1;
        var accountId = 2;
        var fixedDay = new DateTime(2020, 1, 1);
        
        var accountModel = new AccountModel()
        {
            Id = accountId,
            ClientId = clientId,
            Balance = 100,
            Status = AccountStatus.Active,
            AccountType = AccountType.Debit,
            Plan = AccountPlan.Basic,
            CreatedAt = fixedDay,
            AccountNumber = "123"
            
        };

        mockAccountRepository
            .Setup(x => x.GetAccountAsync(
                accountId, 
                clientId))
            .ReturnsAsync(accountModel);

        var accountService = new AccountService(mockAccountRepository.Object, mockLogger.Object);

        var result = await accountService.GetAccountByIdAsync(
            accountId,
            clientId
        );
        
        Assert.NotNull(result);
        Assert.Equal(accountModel.ClientId, result.ClientId);
        Assert.Equal(accountModel.Id, result.AccountId);
        Assert.Equal(accountModel.Balance, result.Balance);
        Assert.Equal(accountModel.Status, result.Status);
        Assert.Equal(accountModel.AccountType, result.AccountType);
        Assert.Equal(accountModel.Plan, result.Plan);
        Assert.Equal(accountModel.CreatedAt, result.CreatedAt);
        Assert.Equal(accountModel.AccountNumber, result.AccountNumber);
        
        mockAccountRepository
            .Verify(x => x.GetAccountAsync(
                    accountId,
                    clientId),
                Times.Once
            );
    }

    [Fact]
    public async Task GetAccountById_ShouldReturnNull_WhenAccountDoesNotExist()
    {
       var mockAccountRepository = new Mock<IAccountRepository>(); 
       var mockLogger = new Mock<ILogger<AccountService>>();
       
       var clientId = 1;
       var accountId = 2;
       
       mockAccountRepository
           .Setup(x => x.GetAccountAsync(
               accountId, 
               clientId))
           .ReturnsAsync((AccountModel?)null);
       
       var service = new AccountService(mockAccountRepository.Object, mockLogger.Object);
       
       var result = await service.GetAccountByIdAsync(
           accountId, 
           clientId
           );
       
           Assert.Null(result);
        
           mockAccountRepository
               .Verify(x => x.GetAccountAsync(
                       accountId, 
                       clientId), 
                   Times.Once
               );
    }

    [Fact]
    public async Task CreateAccount_ShouldReturnResponse_WhenAccountCreated()
    {
        var mockAccountRepository = new Mock<IAccountRepository>();
        var mockLogger = new Mock<ILogger<AccountService>>();

        mockAccountRepository
            .Setup(x => x.CreateAccountAsync(It.IsAny<AccountModel>()))
            .ReturnsAsync(new AccountModel());

        var service = new AccountService(mockAccountRepository.Object, mockLogger.Object);

        var dto = new AccountCreateDto()
        {
            ClientId = 1,
            AccountType = 0
        };

        var result = await service.CreateAccount(dto);

        Assert.NotNull(result);
        Assert.Equal(dto.AccountType, result.AccountType);
    }

    [Fact]
    public async Task GetAllAccountsByClientId_ShouldReturnResponse_WhenClientExists()
    {
        var mockAccountRepository = new Mock<IAccountRepository>();
        var mockLogger = new Mock<ILogger<AccountService>>();
        
        mockAccountRepository
            .Setup(x => x.GetAllAccountsByClientIdAsync(It.IsAny<int>()))
            .ReturnsAsync(new List<AccountModel>
            {
                new AccountModel() { ClientId = 1, AccountNumber = "123" },
                new AccountModel() { ClientId = 1, AccountNumber = "456" }
            });

        var service = new AccountService(mockAccountRepository.Object, mockLogger.Object);

        var result = await service.GetAllAccountsByClientIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("123", result[0].AccountNumber);
        Assert.Equal("456", result[1].AccountNumber);
    }

}