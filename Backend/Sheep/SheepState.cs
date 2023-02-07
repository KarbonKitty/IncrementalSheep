namespace IncrementalSheep;

public record SheepState(int Id, string Name, SheepJobId JobId, JobState JobState);
