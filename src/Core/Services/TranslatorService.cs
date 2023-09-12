using Domain.Entities;
using Domain.Enumerations;
using Shared.Abstraction;
using Shared.Abstraction.Repositories;
using Shared.Exceptions;

namespace Core.Services;

public class TranslatorService
{
    private readonly ITranslatorRepository translatorRepository;
    private readonly IUnitOfWork unitOfWork;

    public TranslatorService(ITranslatorRepository translatorRepository, IUnitOfWork unitOfWork)
    {
        this.translatorRepository = translatorRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task UpdateStatus(int translatorId, TranslatorStatus status)
    {
        var item = await translatorRepository.GetById(translatorId);

        if (item is null)
            throw new NotFoundException(nameof(Translator), translatorId);

        item.Status = status;

        await unitOfWork.SaveChangesAsync();
    }
}
