namespace IncrementalSheep;

public record Upgrade(
    GameObjectId Upgradee,
    UpgradeProperty Property,
    UpgradeType Type,
    SimplePrice Effect);
