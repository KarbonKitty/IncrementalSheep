namespace IncrementalSheep;

public record IdeaState(IdeaId Id, bool IsBought, Lock[] Locks)
    : GameObjectState(Locks);
