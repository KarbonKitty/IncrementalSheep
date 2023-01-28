namespace IncrementalSheep;

public class Sheep
{
    public int Id { get; init; }
    public string Name { get; init; }
    public SheepJob Job { get; private set; }
    public JobState JobState { get; private set; }

    public Sheep(int id, string name, SheepJob job)
    {
        Id = id;
        Name = name;
        Job = job;
        JobState = new(Locked: false);
    }

    public void SwitchJobs(SheepJob newJob)
        => Job = newJob;

    public void LockJob()
        => JobState = new(Locked: true);

    public void UnlockJob()
        => JobState = new(Locked: false);

    public SheepState SaveState()
        => new(Id, Name, Job.Id);
}
