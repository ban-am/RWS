using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction.Repositories;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class TranslatorRepository : ITranslatorRepository
{
    private readonly ApplicationDbContext ctx;

    public TranslatorRepository(ApplicationDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<List<Translator>> GetAll()
    {
        return await GetQuery(null).ToListAsync();
    }

    public async Task<List<Translator>> GetAllByName(string name)
    {
        var items = await GetQuery(null).ToListAsync();

        return items.Where(i => i.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public async Task<Translator> GetById(int id)
    {
        return await GetQuery(i => i.Id == id).FirstOrDefaultAsync();
    }

    private IQueryable<Translator> GetQuery(Expression<Func<Translator, bool>> predicate)
    {
        var query = ctx.Translators.Select(i => i);

        if (predicate is not null)
            query = query.Where(predicate);

        return query;
    }

    public async Task<int> Create(Translator translator)
    {
        ctx.Translators.Add(translator);

        await ctx.SaveChangesAsync();

        return translator.Id;
    }
}
