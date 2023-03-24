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

    public void ApplyUpgrade(SimplePrice upgradeEffect, UpgradeType upgradeType)
    {
        if (upgradeType == UpgradeType.Additive)
        {
            AdditivePrice += upgradeEffect;
        }
        else if (upgradeType == UpgradeType.Multiplicative)
        {
            // TODO: implement
        }
        else
        {
            throw new ArgumentException($"Unhandled UpgradeType: {upgradeType}");
        }
    }

    public SimplePrice Total()
    {
        return BasePrice + AdditivePrice;
    }
}
