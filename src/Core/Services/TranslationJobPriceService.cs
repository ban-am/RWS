namespace Core.Services;

public class TranslationJobPriceService
{
    private const double PricePerCharacter = 0.01;

    public double Calculate(string content)
    {
        return content.Length * PricePerCharacter;
    }
}
