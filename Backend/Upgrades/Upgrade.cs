namespace IncrementalSheep;

public record Upgrade
{
    public GameObjectId Upgradee { get; }
    public UpgradeProperty Property { get; }
    public SimplePrice? AdditiveEffect { get; }
    public PriceMultiplier? MultiplicativeEffect { get; }

    public Upgrade(
        GameObjectId upgradee,
        UpgradeProperty property,
        SimplePrice? additiveEffect = null,
        PriceMultiplier? multiplicativeEffect = null
    )
    {
        if (additiveEffect is null && multiplicativeEffect is null)
        {
            throw new ArgumentException("At least one effect must be set on upgrade");
        }

        Upgradee = upgradee;
        Property = property;
        AdditiveEffect = additiveEffect;
        MultiplicativeEffect = multiplicativeEffect;
    }
}
