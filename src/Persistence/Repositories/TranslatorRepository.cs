using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction.Repositories;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class TranslatorRepository : ITranslatorRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> ctxFactory;

    public TranslatorRepository(IDbContextFactory<ApplicationDbContext> ctxFactory)
    {
        this.ctxFactory = ctxFactory;
    }

    public async Task<List<Translator>> GetAll()
    {
        return await GetQuery(null).ToListAsync();
    }

    public async Task<Translator> GetByName(string name)
    {
        return await GetQuery(i => i.Name == name).FirstOrDefaultAsync();
    }

    public async Task<Translator> GetById(int id)
    {
        return await GetQuery(i => i.Id == id).FirstOrDefaultAsync();
    }

    private IQueryable<Translator> GetQuery(Expression<Func<Translator, bool>> predicate)
    {
        using var ctx = ctxFactory.CreateDbContext();

        var query = ctx.Translators.Select(i => i);

        if (predicate is not null)
            query = query.Where(predicate);

        return query;
    }

    public async Task<int> Create(Translator translator)
    {
        using var ctx = ctxFactory.CreateDbContext();

        ctx.Translators.Add(translator);

        await ctx.SaveChangesAsync();

        return translator.Id;
    }
}
