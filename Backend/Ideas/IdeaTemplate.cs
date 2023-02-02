namespace IncrementalSheep;

public record IdeaTemplate(
    IdeaId Id,
    string Name,
    string Description,
    SimplePrice Price,
    Lock? LockToRemove
);
