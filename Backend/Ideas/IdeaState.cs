namespace IncrementalSheep;

public record IdeaState(GameObjectId Id, bool IsBought, Lock[] Locks)
    : GameObjectState(Locks);
