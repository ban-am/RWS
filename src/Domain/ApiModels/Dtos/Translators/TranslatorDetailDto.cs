using Shared.ApiModels.Dtos.TranslationJobs;

namespace Shared.ApiModels.Dtos.Translators;

public class TranslatorDetailDto : TranslatorDto
{
    public List<TranslationJobRefDto> TranslationJobs { get; set; }
}