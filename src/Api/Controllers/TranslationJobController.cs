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
[ApiVersion("1.0")]
[Route("api/v{api-version:apiVersion}/[controller]")]
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
    public async Task<ActionResult> Create([Required] string customerName, [Required] string contentToTranslate)
    {
        await translationJobService.CreateJob(customerName, contentToTranslate);
        return NoContent();
    }

    [HttpPost("file")]
    public async Task<ActionResult> CreateWithFile(IFormFile file, string customerName)
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

            if (string.IsNullOrEmpty(result.customer) && string.IsNullOrEmpty(customerName))
                return BadRequest($"Failed to retrieve value '{customerName}'.");

            await translationJobService.CreateJob(result.customer ?? customerName , result.content);
        }
        catch (Exception)
        {
            return BadRequest("Invalid input file.");
        }

        return NoContent();
    }

    [HttpPut("{id:int}/status")]
    public async Task<ActionResult> UpdateStatus(int id, JobStatus status)
    {
        if (status == JobStatus.Unknown)
            return BadRequest("Unknown status.");

        try
        {
            await translationJobService.UpdateStatus(id, status);
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }

        return NoContent();
    }


    //    const double PricePerCharacter = 0.01;
    //    private void SetPrice(TranslationJob job)
    //    {
    //        job.Price = job.OriginalContent.Length * PricePerCharacter;
    //    }

    //    [HttpPost]
    //    public bool CreateJob(TranslationJob job)
    //    {
    //        job.Status = "New";
    //        SetPrice(job);
    //        _context.TranslationJobs.Add(job);
    //        bool success = _context.SaveChanges() > 0;
    //        if (success)
    //        {
    //            var notificationSvc = new UnreliableNotificationService();
    //            while (!notificationSvc.SendNotification("Job created: " + job.Id).Result)
    //            {
    //            }

    //            _logger.LogInformation("New job notification sent");
    //        }

    //        return success;
    //    }

    //    [HttpPost]
    //    public bool CreateJobWithFile(IFormFile file, string customer)
    //    {
    //        var reader = new StreamReader(file.OpenReadStream());
    //        string content;

    //        if (file.FileName.EndsWith(".txt"))
    //        {
    //            content = reader.ReadToEnd();
    //        }
    //        else if (file.FileName.EndsWith(".xml"))
    //        {
    //            var xdoc = XDocument.Parse(reader.ReadToEnd());
    //            content = xdoc.Root.Element("Content").Value;
    //            customer = xdoc.Root.Element("Customer").Value.Trim();
    //        }
    //        else
    //        {
    //            throw new NotSupportedException("unsupported file");
    //        }

    //        var newJob = new TranslationJob()
    //        {
    //            OriginalContent = content,
    //            TranslatedContent = "",
    //            CustomerName = customer,
    //        };

    //        SetPrice(newJob);

    //        return CreateJob(newJob);
    //    }

    //    [HttpPost]
    //    public string UpdateJobStatus(int jobId, int translatorId, string newStatus = "")
    //    {
    //        _logger.LogInformation("Job status update request received: " + newStatus + " for job " + jobId.ToString() + " by translator " + translatorId);
    //        if (typeof(JobStatuses).GetProperties().Count(prop => prop.Name == newStatus) == 0)
    //        {
    //            return "invalid status";
    //        }
    //        var job = _context.TranslationJobs.Single(j => j.Id == jobId);
    //        bool isInvalidStatusChange = (job.Status == JobStatuses.New && newStatus == JobStatuses.Completed) ||
    //                                     job.Status == JobStatuses.Completed || newStatus == JobStatuses.New;
    //        if (isInvalidStatusChange)
    //        {
    //            return "invalid status change";
    //        }

    //        job.Status = newStatus;
    //        _context.SaveChanges();
    //        return "updated";
    //    }
}
