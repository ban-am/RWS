namespace Core.Services;

public class TranslationJobPriceCalculatorService
{
    private const double PricePerCharacter = 0.01;

    public double Calculate(string content)
    {
        if (content is null)
            return 0;

        return content.Length * PricePerCharacter;
    }
}
