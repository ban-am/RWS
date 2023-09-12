using AutoMapper;
using Core.Services;
using Domain.Entities;
using Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstraction.Repositories;
using Shared.ApiModels;
using Shared.ApiModels.Dtos;
using System.Data;

namespace Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{api-version:apiVersion}/[controller]")]
public class TranslatorController : ControllerBase
{
    private readonly ITranslatorRepository translatorRepository;
    private readonly IMapper mapper;
    private readonly TranslatorService translatorService;

    public TranslatorController(ITranslatorRepository translatorRepository, IMapper mapper, TranslatorService translatorService)
    {
        this.translatorRepository = translatorRepository;
        this.mapper = mapper;
        this.translatorService = translatorService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<List<TranslatorDto>>> GetTranslators()
    {
        var items = await translatorRepository.GetAll();

        return Ok(items.Select(mapper.Map<TranslatorDto>).ToList());
    }

    [HttpGet("byName")]
    public async Task<ActionResult<TranslatorDto>> GetTranslatorByName(string name)
    {
        var item = await translatorRepository.GetByName(name);

        if (item is null)
            return NoContent();

        return Ok(mapper.Map<TranslatorDto>(item));
    }

    [HttpPost]
    public async Task<ActionResult<int>> AddTranslator([FromBody] CreateTranslatorCommand model)
    {
        var translatorId = await translatorRepository.Create(new Translator
        {
            Name = model.Name,
            HourlyRate = model.HourlyRate,
            CreditCardNumber = model.CreditCardNumber,
            Status = TranslatorStatus.Applicant
        });

        return Ok(translatorId);
    }

    [HttpPut("{id:int}/status")]
    public async Task<ActionResult> UpdateStatus(int id, TranslatorStatus status)
    {
        if (status == TranslatorStatus.Unknown)
            return BadRequest("Unknown status.");

        await translatorService.UpdateStatus(id, status);

        return NoContent();
    }
}
