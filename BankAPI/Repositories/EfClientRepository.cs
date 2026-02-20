using BankAPI.Models.ClientModels;
using BankAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Repositories;

public class EfClientRepository : IClientRepository
{
    private readonly Data.AppDbContext _db;

    public EfClientRepository(Data.AppDbContext db)
    {
        _db = db;
    }

    public async Task<ClientModel> AddClient(ClientModel client)
    {
        await _db.Clients.AddAsync(client);
        _db.SaveChanges();

        return client;
    }

    public async Task<List<ClientModel>> GetAllClients()
    {
        return await _db.Clients
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<ClientModel?> GetClientByIdAsync(int id)
    {
        return await _db.Clients
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<ClientModel?> GetClientByName(string name)
    {
        return await _db.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name);
    }

    //Proxy method
    public Task SaveAsync()
    {
         return _db.SaveChangesAsync();
    }
}