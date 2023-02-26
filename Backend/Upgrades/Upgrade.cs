namespace IncrementalSheep;

public record Upgrade(
    GameObjectId Upgradee,
    UpgradeProperty Property,
    SimplePrice UpgradeEffect);
