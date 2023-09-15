using Domain.Entities;
using Domain.Enumerations;
using Persistence.Repositories;
using Shared.Abstraction;
using Shared.Abstraction.Repositories;
using Shared.Exceptions;

namespace Core.Services;

public class TranslatorService
{
    private readonly ITranslatorRepository translatorRepository;
    private readonly ITranslationJobRepository translationJobRepository;
    private readonly IUnitOfWork unitOfWork;

    public TranslatorService(ITranslatorRepository translatorRepository, ITranslationJobRepository translationJobRepository, IUnitOfWork unitOfWork)
    {
        this.translatorRepository = translatorRepository;
        this.translationJobRepository = translationJobRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task Approve(int translatorId)
    {
        await UpdateStatus(translatorId, TranslatorStatus.Certified);
    }

    public async Task Delete(int translatorId)
    {
        await UpdateStatus(translatorId, TranslatorStatus.Deleted);
    }

    private async Task UpdateStatus(int translatorId, TranslatorStatus status)
    {
        var item = await translatorRepository.GetById(translatorId);

        if (item is null || item.Status == TranslatorStatus.Deleted)
            throw new NotFoundException(nameof(Translator), translatorId);

        item.Status = status;

        await unitOfWork.SaveChangesAsync();
    }

    public async Task AssignJob(int translatorId, int jobId)
    {
        var translator = await translatorRepository.GetById(translatorId);

        if (translator is null)
            throw new NotFoundException(nameof(Translator), translatorId);

        if (translator.Status != TranslatorStatus.Certified)
            throw new InvalidOperationException("Only Certified translators can work on jobs.");

        var job = await translationJobRepository.GetById(jobId);

        if (job is null)
            throw new NotFoundException(nameof(TranslationJob), jobId);

        job.TranslatorId = translatorId;

        await unitOfWork.SaveChangesAsync();
    }
}
