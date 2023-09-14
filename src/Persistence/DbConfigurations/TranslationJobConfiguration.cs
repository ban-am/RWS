using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.DbConfigurations;

public class TranslationJobConfiguration : IEntityTypeConfiguration<TranslationJob>
{
    public void Configure(EntityTypeBuilder<TranslationJob> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(i => i.Id)
               .ValueGeneratedOnAdd();

        builder.HasOne(tj => tj.Translator)
               .WithMany(t => t.TranslationJobs)
               .HasForeignKey(tj => tj.TranslatorId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
