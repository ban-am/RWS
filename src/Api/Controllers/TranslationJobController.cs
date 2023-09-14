using AutoMapper;
using Core.Handlers;
using Core.Services;
using Domain.Entities;
using Domain.Enumerations;
using External.ThirdParty.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Abstraction.Repositories;
using Shared.ApiModels;
using Shared.ApiModels.Dtos;
using Shared.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
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

        return Ok(items.Select(mapper.Map<TranslationJobDto>).ToList());
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

        try
        {
            using var fileStream = file.OpenReadStream();
            var result = await handler.HandleFileAsync(fileStream);

            customerName ??= result.customer;

            if (string.IsNullOrEmpty(result.content))
                return BadRequest("Failed to load 'content' value.");

            if (string.IsNullOrEmpty(customerName))
                return BadRequest("Failed to load 'customerName' value.");

            return await translationJobService.CreateJob(customerName, result.content);
        }
        catch (Exception)
        {
            return BadRequest("Invalid input file.");
        }
    }

    [HttpPut("{jobId:int}/status")]
    public async Task<ActionResult> UpdateStatus(int jobId, JobStatus status)
    {
        try
        {
            await translationJobService.UpdateStatus(jobId, status);
            return NoContent();
        }
        catch (Exception e) when (e is InvalidOperationException || e is NotFoundException)
        {
            return BadRequest(e.Message);
        }

    }
}
