using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Shared.Abstraction.Repositories;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class TranslationJobRepository : ITranslationJobRepository
{
    private readonly IDbContextFactory<ApplicationDbContext> ctxFactory;

    public TranslationJobRepository(IDbContextFactory<ApplicationDbContext> ctxFactory)
    {
        this.ctxFactory = ctxFactory;
    }

    public async Task<List<TranslationJob>> GetJobs()
    {
        return await GetQuery(null).ToListAsync();
    }

    public async Task<TranslationJob> GetById(int id)
    {
        return await GetQuery(i => i.Id == id).FirstOrDefaultAsync();
    }

    private IQueryable<TranslationJob> GetQuery(Expression<Func<TranslationJob, bool>> predicate)
    {
        using var ctx = ctxFactory.CreateDbContext();

        var query = ctx.TranslationJobs.Select(i => i);

        if (predicate is not null)
            query = query.Where(predicate);

        return query;
    }

    public async Task<int> CreateJob(TranslationJob job)
    {
        using var ctx = ctxFactory.CreateDbContext();

        ctx.TranslationJobs.Add(job);

        await ctx.SaveChangesAsync();

        return job.Id;
    }
}
