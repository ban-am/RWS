namespace Shared.ApiModels;

public class CreateTranslatorCommand
{
    public string Name { get; set; }
    public string HourlyRate { get; set; }
    public string CreditCardNumber { get; set; }
}