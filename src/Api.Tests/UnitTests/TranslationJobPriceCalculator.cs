using Core.Services;

namespace Api.Tests.UnitTests;

public class TranslationJobPriceCalculator
{
    private readonly TranslationJobPriceCalculatorService calculatorService;

    public TranslationJobPriceCalculator()
    {
        calculatorService = new TranslationJobPriceCalculatorService();
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void Price_Should_Be_Equal(string text, double expected)
    {
        Assert.Equal(expected, calculatorService.Calculate(text));
    }

    public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { null, 0 },
            new object[] { "", 0 },
            new object[] { GetText(1), 1 * 0.01 },
            new object[] { GetText(1_000_000), 1_000_000 * 0.01 }
        };

    private static string GetText(int length) => new string('-', length);

}
