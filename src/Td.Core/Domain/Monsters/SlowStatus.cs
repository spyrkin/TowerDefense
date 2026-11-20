namespace Td.Core.Domain.Monsters;

public sealed record SlowStatus
{
    public required double Percent { get; init; }

    public required double RemainingSeconds { get; init; }
}