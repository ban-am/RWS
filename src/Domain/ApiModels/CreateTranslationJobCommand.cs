using Domain.Enumerations;

namespace Shared.ApiModels;

public class CreateTranslationJobCommand
{
    public string CustomerName { get; set; }
    public string OriginalContent { get; set; }
}
