using Microsoft.EntityFrameworkCore;
using BankAPI.Models.ClientModels;
using BankAPI.Models;

namespace BankAPI.Data;

public class AppDbContext : DbContext
{
    public DbSet<ClientModel> Clients { get; set; }
    public DbSet<AccountModel> Accounts { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : 
        base(options){}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}