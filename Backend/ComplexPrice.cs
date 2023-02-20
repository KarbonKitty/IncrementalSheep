namespace IncrementalSheep;

public class ComplexPrice
{
    private SimplePrice BasePrice { get; }
    private SimplePrice AdditivePrice { get; }

    public ComplexPrice(
        SimplePrice basePrice,
        SimplePrice? additivePrice = null
    )
    {
        BasePrice = basePrice;
        AdditivePrice = additivePrice ?? new();
    }

    public SimplePrice Total()
    {
        return BasePrice + AdditivePrice;
    }
}
