namespace IncrementalSheep;

public record IdeaTemplate(
    GameObjectId Id,
    string Name,
    string Description,
    SimplePrice Price,
    Lock? LockToRemove,
    Lock[] Locks
);
