using AutoMapper;
using Domain.Entities;
using Shared.ApiModels.Dtos;

namespace Api.Configurations;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TranslationJob, TranslationJobDto>();
        CreateMap<Translator, TranslatorDto>();
    }
}