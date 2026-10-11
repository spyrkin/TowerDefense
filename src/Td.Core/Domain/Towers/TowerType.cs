public sealed record TowerType
{
    public required string Id { get; init; }
    public required string Name { get; init; }

    public required TowerKind Kind { get; init; }

    public required int BaseCost { get; init; }

    public required IReadOnlyList<TowerLevelDefinition> Levels { get; init; }
}