public sealed record TowerType
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required TowerKind Kind { get; init; }

    public required int BaseCost { get; init; }

    public required IReadOnlyList<TowerLevelDefinition> Levels { get; init; }

    public double FootprintRadius { get; init; } = 24.0;

    /// <summary>
    /// Может ли башня атаковать летающие цели.
    /// По умолчанию разрешено, чтобы не ломать существующий контент.
    /// </summary>
    public bool CanTargetFlying { get; init; } = true;
}