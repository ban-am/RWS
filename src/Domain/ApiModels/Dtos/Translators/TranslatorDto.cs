using Domain.Enumerations;

namespace Shared.ApiModels.Dtos.Translators;

public class TranslatorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal HourlyRate { get; set; }
    public TranslatorStatus Status { get; set; }
}
