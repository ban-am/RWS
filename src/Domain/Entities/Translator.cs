using Domain.Enumerations;

namespace Domain.Entities;

public class Translator
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string HourlyRate { get; set; }
    public TranslatorStatus Status { get; set; }
    public string CreditCardNumber { get; set; }

    public ICollection<TranslationJob> TranslationJobs { get; set; }
}

