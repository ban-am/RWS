using Domain.Entities;
using Domain.Enumerations;
using External.ThirdParty.Services;
using Persistence.Repositories;
using Shared.Abstraction;
using Shared.Abstraction.Repositories;
using Shared.Exceptions;

namespace Core.Services;

public class TranslationJobService
{
    private readonly ITranslationJobRepository translationJobRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly UnreliableNotificationService notificationService;
    private readonly TranslationJobPriceService translationJobPriceService;

    public TranslationJobService(ITranslationJobRepository translationJobRepository, IUnitOfWork unitOfWork, UnreliableNotificationService notificationService, TranslationJobPriceService translationJobPriceService)
    {
        this.translationJobRepository = translationJobRepository;
        this.unitOfWork = unitOfWork;
        this.notificationService = notificationService;
        this.translationJobPriceService = translationJobPriceService;
    }

    public async Task CreateJob(string customerName, string contentToTranslate)
    {
        var jobId = await translationJobRepository.CreateJob(new TranslationJob
        {
            CustomerName = customerName,
            OriginalContent = contentToTranslate,
            Status = JobStatus.New,
            Price = translationJobPriceService.Calculate(contentToTranslate),
        });

        await notificationService.SendNotification($"Job created: {jobId}");
    }

    public async Task UpdateStatus(int jobId, JobStatus status)
    {
        var item = await translationJobRepository.GetById(jobId);

        if (item is null)
            throw new NotFoundException(nameof(TranslationJob), jobId);

        item.Status = status;

        await unitOfWork.SaveChangesAsync();
    }
}
