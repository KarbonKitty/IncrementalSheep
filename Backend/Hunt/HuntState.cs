namespace IncrementalSheep;

public record HuntState(HuntId Id, TimeSpan TimeLeft, Lock[] Locks)
    : GameObjectState(Locks);
