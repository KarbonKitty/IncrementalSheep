namespace IncrementalSheep;

public interface ITakeTime
{
    TimeSpan Duration { get; }
    TimeSpan TimeLeft { get; }
    bool Start();
    bool SpendTime(TimeSpan deltaT);
}
