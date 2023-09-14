using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DbConfigurations;

public class TranslatorConfiguration : IEntityTypeConfiguration<Translator>
{
    public void Configure(EntityTypeBuilder<Translator> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.HourlyRate)
            .HasColumnType("decimal(18,2)");
    }
}
