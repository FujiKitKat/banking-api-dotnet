using BankAPI.DTO.ClientDTO;
using BankAPI.Enum;

namespace BankAPI.Services.Interfaces;

public interface IClientService
{
    Task<bool> ClientUpdateStatusAsync(int id,  ClientStatus status);
    Task<List<ClientResponseDTO>> GetActiveСlientsAsync();
    Task<ClientResponseDTO> CreateClientAsync(ClientCreateDTO clientCreateDto);
    Task<ClientResponseDTO?> GetClientByIdAsync(int id);
    Task<ClientResponseDTO?> UpdateClientAsync(int id, ClientUpdateDTO clientUpdateDto);
    Task<IEnumerable<ClientResponseDTO>> GetAllClientsAsync();
    Task<ClientResponseDTO?> GetClientByNameAsync(string name);
}