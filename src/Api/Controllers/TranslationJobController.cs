using Api.Configurations;
using AutoMapper;
using Core.Handlers;
using Core.Services;
using Domain.Entities;
using Domain.Enumerations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Shared.Abstraction.Repositories;
using Shared.ApiModels.Dtos.TranslationJobs;
using Shared.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[EnableCors(EnableCors.AllowAllHeaders)]
public class TranslationJobController : ControllerBase
{
    private readonly ITranslationJobRepository jobRepository;
    private readonly IMapper mapper;
    private readonly TranslationJobService translationJobService;
    private readonly FileHandlerFactory fileHandlerFactory;

    public TranslationJobController(ITranslationJobRepository jobRepository, IMapper mapper, TranslationJobService translationJobService, FileHandlerFactory fileHandlerFactory)
    {
        this.jobRepository = jobRepository;
        this.mapper = mapper;
        this.translationJobService = translationJobService;
        this.fileHandlerFactory = fileHandlerFactory;
    }

    [HttpGet("list")]
    public async Task<ActionResult<List<TranslationJobDto>>> GetAll()
    {
        var items = await jobRepository.GetJobs();
        return Ok(mapper.Map<List<TranslationJobDto>>(items));
    }

    [HttpGet("{jobId:int}")]
    public async Task<ActionResult<TranslationJobDetailDto>> GetById(int jobId)
    {
        var item = await jobRepository.GetById(jobId);

        if (item is null)
            return NotFound($"Entity \"{nameof(TranslationJob)}\" ({jobId}) was not found.");

        return Ok(mapper.Map<TranslationJobDetailDto>(item));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([Required] string customerName, [Required] string contentToTranslate)
    {
        return await translationJobService.CreateJob(customerName, contentToTranslate);
    }

    [HttpPost("file")]
    public async Task<ActionResult<int>> CreateWithFile(IFormFile file, string customerName)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var fileType = Path.GetExtension(file.FileName);

        if (!fileHandlerFactory.TryCreateHandler(fileType, out var handler))
            return BadRequest("Unsupported file type.");

        (string content, string customer) result = default;

        try
        {
            using var fileStream = file.OpenReadStream();
            result = await handler.HandleFileAsync(fileStream);

            customerName ??= result.customer;

            if (string.IsNullOrEmpty(result.content))
                return BadRequest("Failed to load 'content' value.");

            if (string.IsNullOrEmpty(customerName))
                return BadRequest("Failed to load 'customerName' value.");
        }
        catch (Exception)
        {
            return BadRequest("Invalid input file.");
        }

        return await translationJobService.CreateJob(customerName, result.content);
    }

    [HttpPut("{jobId:int}/status")]
    public async Task<ActionResult> UpdateStatus(int jobId, JobStatus status)
    {
        try
        {
            await translationJobService.UpdateStatus(jobId, status);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }

    }
}
