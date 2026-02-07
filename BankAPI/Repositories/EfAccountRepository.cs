using BankAPI.Enum;
using BankAPI.Models;
using BankAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Repositories;
public class EfAccountRepository : IAccountRepository
{
    private readonly Data.AppDbContext _db;

    public EfAccountRepository(Data.AppDbContext db)
    {
        _db = db;
    }
    
    public async Task<AccountModel?> GetAccountAsync(int id)
    {
        return await _db.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<AccountModel>> GetAllAccountsByClientIdAsync(int  clientId)
    {
        return await  _db.Accounts
            .AsNoTracking()
            .Where(x => x.ClientId == clientId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<AccountModel> CreateAccountAsync(AccountModel account)
    {
        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();
        
        return account;
    }

    public async Task<AccountModel?> UpdateAccountStatusAsync(AccountModel account)
    {
       var existAccount = await _db.Accounts
           .FirstOrDefaultAsync(x => x.Id == account.Id);

       if (existAccount == null)
       {
           return null;
       }
       
       await _db.SaveChangesAsync();
       
       return existAccount;
    }

    public Task SaveAsync()
    {
        return _db.SaveChangesAsync();
    }
    
}