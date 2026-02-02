using BankAPI.Enum;
using Microsoft.EntityFrameworkCore;
using BankAPI.Models.ClientModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAPI.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<ClientModel>
{
    public void Configure(EntityTypeBuilder<ClientModel> builder)
    {
        builder.Property(x =>x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.HasIndex(x => x.Email)
            .IsUnique();
        
        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasDefaultValue(ClientStatus.Active)
            .IsRequired();
        
        builder.Property(x => x.CreateDate)
            .IsRequired();
    }
}