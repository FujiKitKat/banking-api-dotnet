using BankAPI.Models.ClientModels;

namespace BankAPI.Repositories.Interfaces;

public interface IClientRepository
{
    Task<ClientModel> AddClient(ClientModel client);
    Task <List<ClientModel>> GetAllClients();
    Task<ClientModel?> GetClientByIdAsync(int id);
    Task<ClientModel?> GetClientByName(string name);
    Task<bool> DeleteClient (int id);
    Task SaveAsync();

}