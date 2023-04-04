namespace IncrementalSheep;

public record SheepJobState(GameObjectId Id, Lock[] Locks)
    : GameObjectState(Locks);
