using AutoMapper;
using Domain.Entities;
using Shared.ApiModels.Dtos.TranslationJobs;
using Shared.ApiModels.Dtos.Translators;

namespace Api.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TranslationJob, TranslationJobDto>();
        CreateMap<TranslationJob, TranslationJobRefDto>();
        CreateMap<TranslationJob, TranslationJobDetailDto>();

        CreateMap<Translator, TranslatorDto>();
        CreateMap<Translator, TranslatorDetailDto>();
    }
}
