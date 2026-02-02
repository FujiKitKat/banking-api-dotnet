using BankAPI.DTO.ClientDTO;
using BankAPI.Services.Interfaces;
using BankAPI.Enum;
using BankAPI.Models;
using BankAPI.Repositories.Interfaces;
using BankAPI.Models.ClientModels;

namespace BankAPI.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<bool> ClientUpdateStatusAsync(int id, ClientStatus status)
    {
        var client = await _clientRepository.GetClientByIdAsync(id);

        if (client == null)
        {
            return false;
        }
        
        client.Status = status;

        await _clientRepository.SaveAsync();
        
        return true;
    }

    public async Task<List<ClientResponseDTO>> GetActiveAclientsAsync()
    {
        var clients = await _clientRepository.GetAllClients();

        return  clients
            .Where(x => x.Status == ClientStatus.Active)
            .Select(x => new ClientResponseDTO
            {
                Id = x.Id,
                Name = x.Name,
                Status = x.Status,
                
            })
            .ToList();
    }

    public async Task<ClientResponseDTO> CreateClientAsync(ClientCreateDTO clientCreateDto)
    {
        var normalizeEmail = clientCreateDto.Email.Trim().ToLowerInvariant();
        
        ClientModel clientModel = new ClientModel
        {
            CreateDate = DateTime.UtcNow,
            Status = ClientStatus.Active,
            Accounts = new List<AccountModel>(),
            Name = clientCreateDto.Name.Trim(),
            Email = normalizeEmail,
            PhoneNumber = clientCreateDto.PhoneNumber.Trim()
        };

        var createdClient = await _clientRepository.AddClient(clientModel);

        var response = new ClientResponseDTO
        {
            Id = createdClient.Id,
            Name = createdClient.Name,
            Email = createdClient.Email,
            PhoneNumber = createdClient.PhoneNumber,
            Status = createdClient.Status
        };
        
        await _clientRepository.SaveAsync();
        
        return response;
    }

    public async Task<ClientResponseDTO?> GetClientByIdAsync(int id)
    {
        var client = await _clientRepository.GetClientByIdAsync(id);
        
        if(client == null)
        {
            return null;
        }

        var response = new ClientResponseDTO
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Status = client.Status
        };
        
        return response;
    }

    public async Task<ClientResponseDTO?> UpdateClientAsync(int id, ClientUpdateDTO clientUpdateDto)
    {
        var  client = await _clientRepository.GetClientByIdAsync(id);

        if (client == null)
        {
            return null;
        }

        client.Name = clientUpdateDto.Name ?? client.Name;
        client.Email = clientUpdateDto.Email ?? client.Email;
        client.PhoneNumber = clientUpdateDto.PhoneNumber ?? client.PhoneNumber;
        
        await _clientRepository.SaveAsync();

        return new ClientResponseDTO
        {
            Name = client.Name,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
        };
    }

    public async Task<IEnumerable<ClientResponseDTO>> GetAllClientsAsync()
    {
        var clients = await _clientRepository.GetAllClients();
        
        return clients.Select(x => new ClientResponseDTO
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            Status = x.Status
            
        }).ToList();
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        var client = await _clientRepository.GetClientByIdAsync(id);

        if (client == null)
        {
            return false;
        }

        await _clientRepository.DeleteClient(id);
        await _clientRepository.SaveAsync();

        return true;
    }

    public async Task<ClientResponseDTO?> GetClientByNameAsync(string name)
    {
        var client = await _clientRepository.GetClientByName(name);

        if (client == null)
        {
            return null;
        }

        var response = new ClientResponseDTO
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            PhoneNumber = client.PhoneNumber,
            Status = client.Status
        };
        
        return response;
    }
}