using Domain.Enumerations;

namespace Shared.ApiModels.Dtos.TranslationJobs;

public class TranslationJobRefDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public JobStatus Status { get; set; }
    public double Price { get; set; }
}
