public sealed record MonsterType
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required MonsterClass Class { get; init; }

    public required int MaxHp { get; init; }

    public required double Speed { get; init; }

    public required int Armor { get; init; }

    public required int GoldReward { get; init; }

    public required double SlowResistance { get; init; }

    public required int Tier { get; init; }

    /// <summary>
    /// Является ли монстр воздушной целью.
    /// </summary>
    public bool IsFlying => Class == MonsterClass.Flying;
}