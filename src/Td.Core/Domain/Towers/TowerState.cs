public sealed class TowerState
{
    public required Guid Id { get; init; }
    public required TowerType Type { get; init; }

    public required Point2D Position { get; set; }

    public int Level { get; set; } = MechanicsLimits.MinUpgradeLevel;

    public int InvestedGold { get; set; }
}