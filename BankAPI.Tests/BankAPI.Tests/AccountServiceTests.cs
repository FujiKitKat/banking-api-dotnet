using Moq;
using BankAPI.DTO.AccountDTO;
using BankAPI.Repositories.Interfaces;
using BankAPI.Models;
using BankAPI.Services;
using Microsoft.Extensions.Logging;

namespace BankAPI.Tests;

public class AccountServiceTests
{
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