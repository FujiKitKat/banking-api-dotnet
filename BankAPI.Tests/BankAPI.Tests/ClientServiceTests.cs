using BankAPI.DTO.ClientDTO;
using BankAPI.Enum;
using BankAPI.Repositories.Interfaces;
using Moq;
using BankAPI.Services;
using Microsoft.Extensions.Logging;
using BankAPI.Models.ClientModels;

namespace BankAPI.Tests;

public class ClientServiceTests
{
    [Fact]
    public async Task CreateClientAsync_ShouldReturnResponse_WhenClientCreated()
    {
        var mockClientRepository = new Mock<IClientRepository>();
        var mockLogger = new Mock<ILogger<ClientService>>();
        
        var dto = new ClientCreateDTO
        {
            Name = "James",
            Email = "james123@gmail.com",
            PhoneNumber = "8725646464",
        };

        var createdModel = new ClientModel
        {
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };
        
        mockClientRepository
            .Setup(x => x.AddClient(It.IsAny<ClientModel>()))
            .ReturnsAsync(createdModel);
        
        var service = new ClientService(mockClientRepository.Object, mockLogger.Object);

        var result = await service.CreateClientAsync(dto);
        
        Assert.NotNull(result);
        
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(dto.PhoneNumber, result.PhoneNumber);
    }

    [Fact]
    public async Task GetClientAsync_ShouldReturnResponse_WhenClientExists()
    {
        var mockClientRepository = new Mock<IClientRepository>();
        var mockLogger = new Mock<ILogger<ClientService>>();

        var clientId = 1;

        var clientModel = new ClientModel
        {
            Id = clientId,
            Name = "James",
        };
        
        mockClientRepository
            .Setup(x => x.GetClientByIdAsync(clientId))
            .ReturnsAsync(clientModel);
        
        var service = new ClientService(mockClientRepository.Object, mockLogger.Object);
        
        var result = await service.GetClientByIdAsync(clientId);
        
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetAllClientsAsync_ShouldReturnResponse_WhenClientExists()
    {
        var mockClientRepository = new Mock<IClientRepository>();
        var mockLogger = new Mock<ILogger<ClientService>>();

        var clientModel = new List<ClientModel>
        {
            new ClientModel() { Id = 1, Name = "James" },
            new ClientModel() { Id = 2, Name = "James2" }
        };
        
        mockClientRepository
            .Setup(x => x.GetAllClients())
            .ReturnsAsync(clientModel);
        
        var service = new ClientService(mockClientRepository.Object, mockLogger.Object);
        
        var result = await service.GetAllClientsAsync();
        
        var resultList = result.ToList();
        
        Assert.NotNull(result);
        Assert.Equal(2, resultList.Count);
        Assert.Equal("James", resultList[0].Name);
        Assert.Equal("James2", resultList[1].Name);
    }

    [Fact]
    public async Task GetClientByNameAsync_ShouldReturnResponse_WhenClientExists()
    {
        var mockClientRepository = new Mock<IClientRepository>();
        var mockLogger = new Mock<ILogger<ClientService>>();

        var name = "James";
        
        var clientModel = new ClientModel
        {
            Name = "James",
        };
        
        mockClientRepository
            .Setup(x => x.GetClientByName(name))
            .ReturnsAsync(clientModel);
        
        var service = new ClientService(mockClientRepository.Object, mockLogger.Object);
        
        var result = await service.GetClientByNameAsync(name);
        
        Assert.NotNull(result);
        Assert.Equal(clientModel.Name, result.Name);
        
        mockClientRepository
            .Verify(x => x.GetClientByName(name), 
                Times.Once);
    }

    [Fact]
    public async Task UpdateClientAsync_ShouldReturnResponse_WhenClientUpdated()
    {
        var mockClientRepository = new Mock<IClientRepository>();
        var mockLogger = new Mock<ILogger<ClientService>>();
        
        var clientId = 1;
        var clientStatus = ClientStatus.Blocked;
        
        mockClientRepository
            .Setup(x => x.GetClientByIdAsync(clientId))
            .ReturnsAsync(new ClientModel());
        
        var service = new ClientService(mockClientRepository.Object, mockLogger.Object);

        var result = await service.ClientUpdateStatusAsync(clientId, clientStatus);
        
        Assert.True(result);
    }

    [Fact]
    public async Task GetAllActiveClientsAsync_ShouldReturnResponse_WhenActiveClientExists()
    {
        var mockClientRepository = new Mock<IClientRepository>();
        var mockLogger = new Mock<ILogger<ClientService>>();

        var clientModel = new List<ClientModel>
        {
            new ClientModel() 
            { 
                Id = 1, 
                Name = "James", 
                Status = ClientStatus.Active
            },
            
            new ClientModel()
            {
                Id = 2, 
                Name = "James2", 
                Status = ClientStatus.Active
            },
            
            new ClientModel()
            {
                Id = 3,
                Name = "James3",
                Status = ClientStatus.Blocked
            },
            
            new ClientModel()
            {
                Id = 4,
                Name = "James4",
                Status = ClientStatus.Suspended
            }
        };
        
        mockClientRepository
            .Setup(x => x.GetAllClients())
            .ReturnsAsync(clientModel);
        
        var service = new ClientService(mockClientRepository.Object, mockLogger.Object);

        var result = await service.GetActiveСlientsAsync();
        
        var resultList = result.ToList();
        
        Assert.NotNull(result);
        Assert.Equal(2, resultList.Count);
        Assert.All(resultList, x => Assert.Equal(ClientStatus.Active, x.Status));
        
        mockClientRepository.Verify(x => x.GetAllClients(),Times.Once);
    }
    
    [Fact]
    public async Task GetClientByIdAsync_ShouldReturnClient_WhenClientExists()
    {
        var mockClientRepository = new Mock<IClientRepository>();
        var  mockLogger = new Mock<ILogger<ClientService>>();

        var clientId = 1;

        var clientModel = new ClientModel
        {
            Id = clientId
        };
        
        mockClientRepository
            .Setup(x => x.GetClientByIdAsync(clientId))
            .ReturnsAsync(clientModel);
        
        var service = new ClientService(mockClientRepository.Object, mockLogger.Object);

        var result = await service.GetClientByIdAsync(clientId);
        
        Assert.NotNull(result);
        Assert.Equal(clientModel.Id, result.Id);
        
        mockClientRepository
            .Verify(x => x.GetClientByIdAsync(clientId),
                Times.Once);
    }

    [Fact]
    public async Task GetClientByIdAsync_ShouldReturnNull_WhenClientDoesNotExist()
    {
        var mockClientRepository = new Mock<IClientRepository>();
        var mockLogger = new Mock<ILogger<ClientService>>();
        
        mockClientRepository
            .Setup(x => x.GetClientByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((ClientModel?)null);

        var service = new ClientService(mockClientRepository.Object, mockLogger.Object);

        var result = await service.GetClientByIdAsync(999);
        
        Assert.Null(result);
        
        mockClientRepository
            .Verify(x => x.GetClientByIdAsync(It.IsAny<int>()),
                Times.Once);
    }
}