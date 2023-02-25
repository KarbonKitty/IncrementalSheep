namespace IncrementalSheep;

public class ComplexPrice
{
    private SimplePrice BasePrice { get; }
    private SimplePrice AdditivePrice { get; set; }

    public ComplexPrice(
        SimplePrice basePrice,
        SimplePrice? additivePrice = null
    )
    {
        BasePrice = basePrice;
        AdditivePrice = additivePrice ?? new();
    }

    public void AddBonus(SimplePrice bonus)
        => AdditivePrice += bonus;

    public SimplePrice Total()
    {
        return BasePrice + AdditivePrice;
    }
}
