using AutoMapper;
using Core.Services;
using Domain.Entities;
using Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstraction.Repositories;
using Shared.ApiModels;
using Shared.ApiModels.Dtos.Translators;
using Shared.Exceptions;
using System.Data;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
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

        return Ok(mapper.Map<List<TranslatorDto>>(items));
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<TranslatorDto>>> GetTranslatorsByName(string name)
    {
        var items = await translatorRepository.GetAllByName(name);

        if (items is null)
            return NoContent();

        return Ok(mapper.Map<List<TranslatorDto>>(items));
    }

    [HttpGet("{translatorId:int}")]
    public async Task<ActionResult<TranslatorDetailDto>> GetTranslatorDetail(int translatorId)
    {
        var items = await translatorRepository.GetById(translatorId);

        if (items is null)
            return NotFound($"Entity \"{nameof(Translator)}\" ({translatorId}) was not found.");

        return Ok(mapper.Map<TranslatorDetailDto>(items));
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

    [HttpPut("{translatorId:int}/approve")]
    public async Task<ActionResult> Approve(int translatorId)
    {
        try
        {
            await translatorService.Approve(translatorId);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }

        return NoContent();
    }

    [HttpDelete("{translatorId:int}")]
    public async Task<ActionResult> Delete(int translatorId)
    {
        try
        {
            await translatorService.Delete(translatorId);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }

        return NoContent();
    }

    [HttpPut("{translatorId:int}/job")]
    public async Task<ActionResult> AssignJob(int translatorId, int jobId)
    {
        try
        {
            await translatorService.AssignJob(translatorId, jobId);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }

        return NoContent();
    }
}
