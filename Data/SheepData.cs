namespace IncrementalSheep;

public static class SheepData {
    public static readonly SimplePrice NewSheepBasePrice = new(ResourceId.Food, 50);

    public static readonly SimplePrice SheepBaseConsumption = new(ResourceId.Food, 2);

    public static readonly string[] Names = {
        "Norman",
        "Cooper",
        "Raymond",
        "Lars",
        "Isabelle",
        "Molly",
        "Calliope",
        "Blossom",
        "Penny",
        "Gizmo",
        "Kenny",
        "Lambert"
    };
}
