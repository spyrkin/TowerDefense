public sealed class MonsterState
{
    public required Guid Id { get; init; }
    public required MonsterType Type { get; init; }

    public required double CurrentHp { get; set; }
    public required Point2D Position { get; set; }

    public double PathProgress { get; set; }

    public SlowStatus? Slow { get; set; }

    public bool IsAlive => CurrentHp > 0;
}