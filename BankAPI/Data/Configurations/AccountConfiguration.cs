using BankAPI.Enum;
using BankAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAPI.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<AccountModel>
{
    public void Configure(EntityTypeBuilder<AccountModel> builder)
    {
        builder.Property(x => x.AccountNumber)
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(x => x.AccountNumber)
            .IsUnique();
        
        builder.Property(x => x.Balance)
            .HasDefaultValue(0)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasDefaultValue(AccountStatus.Active)
            .IsRequired();

        builder.Property(x => x.AccountType)
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();
        
        builder.HasOne(x => x.Client)
            .WithMany(x => x.Accounts)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}