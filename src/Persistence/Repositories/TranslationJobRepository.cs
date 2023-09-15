using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction.Repositories;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class TranslationJobRepository : ITranslationJobRepository
{
    private readonly ApplicationDbContext ctx;

    public TranslationJobRepository(ApplicationDbContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<List<TranslationJob>> GetJobs()
    {
        return await GetQuery(null).ToListAsync();
    }

    public async Task<TranslationJob> GetById(int id)
    {
        return await GetQuery(i => i.Id == id)
            .Include(i => i.Translator)
            .FirstOrDefaultAsync();
    }

    private IQueryable<TranslationJob> GetQuery(Expression<Func<TranslationJob, bool>> predicate)
    {
        var query = ctx.TranslationJobs.Select(i => i);

        if (predicate is not null)
            query = query.Where(predicate);

        return query;
    }

    public async Task<int> CreateJob(TranslationJob job)
    {
        ctx.TranslationJobs.Add(job);

        await ctx.SaveChangesAsync();

        return job.Id;
    }
}
