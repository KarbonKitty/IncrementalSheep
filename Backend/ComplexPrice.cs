namespace IncrementalSheep;

public class ComplexPrice
{
    private SimplePrice BasePrice { get; }
    private SimplePrice AdditivePrice { get; set; }
    private PriceMultiplier Multiplier { get; }

    private SimplePrice CachedTotal = new();

    private bool dirty = true;

    public ComplexPrice(
        SimplePrice basePrice,
        SimplePrice? additivePrice = null,
        PriceMultiplier? multiplier = null
    )
    {
        BasePrice = basePrice;
        AdditivePrice = additivePrice ?? new();
        Multiplier = multiplier ?? new();
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        dirty = true;
        if (upgrade.AdditiveEffect is not null)
        {
            ApplyAdditiveUpgrade(upgrade.AdditiveEffect);
        }
        if (upgrade.MultiplicativeEffect is not null)
        {
            ApplyMultiplicativeUpgrade(upgrade.MultiplicativeEffect);
        }
    }

    public SimplePrice Total()
        => dirty ? Calculate() : CachedTotal;

    private void ApplyAdditiveUpgrade(SimplePrice upgradeEffect)
        => AdditivePrice += upgradeEffect;

    private void ApplyMultiplicativeUpgrade(PriceMultiplier multiplier)
        => Multiplier.AddMultiplier(multiplier);

    private SimplePrice Calculate()
    {
        dirty = false;
        CachedTotal = (BasePrice + AdditivePrice) * Multiplier;
        return CachedTotal;
    }
}
