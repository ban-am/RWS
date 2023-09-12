using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction;
using System.Reflection;

namespace Persistence;

public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    // dotnet ef migrations add "Init" --project src\Persistence --startup-project src\Api --output-dir Migrations
    // dotnet ef database update --project src\Persistence  --startup-project src\Api

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<TranslationJob> TranslationJobs { get; set; }
    public DbSet<Translator> Translators { get; set; }
}