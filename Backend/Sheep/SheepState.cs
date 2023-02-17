namespace IncrementalSheep;

public record SheepState(int Id, string Name, GameObjectId JobId, JobState JobState);
