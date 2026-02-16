using Festpay.Onboarding.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Festpay.Onboarding.Infra.Configurations;

public class AccountConfiguration : ConfigurationBase<Account>, IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        ConfigureEntityBase(builder);
        builder
            .HasIndex(a => a.Document)
            .IsUnique();
        builder
            .Property(a => a.Version)
            .IsConcurrencyToken();
    }
}
