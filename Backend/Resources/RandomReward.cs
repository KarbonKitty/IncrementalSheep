namespace IncrementalSheep;

public class RandomReward
{
    public required RandomRewardItem[] Items { get; init; }
}

public record RandomRewardItem(
    ResourceId Resource,
    double Minimum,
    double Maximum,
    double Chance);
