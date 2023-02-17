namespace IncrementalSheep;

public record HuntState(GameObjectId Id, TimeSpan TimeLeft, Lock[] Locks)
    : GameObjectState(Locks);
