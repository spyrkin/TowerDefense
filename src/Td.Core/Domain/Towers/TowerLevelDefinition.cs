public sealed record TowerLevelDefinition
{
    public required int Level { get; init; }

    public required int UpgradeCost { get; init; }

    public required int Damage { get; init; }
    public required double Range { get; init; }
    public required double CooldownSeconds { get; init; }

    public required double SlowPercent { get; init; }
    public required double SlowDurationSeconds { get; init; }

    public required double SplashRadius { get; init; }

    public required double ArmorIgnorePercent { get; init; }
}