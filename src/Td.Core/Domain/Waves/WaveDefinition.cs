public sealed record WaveDefinition
{
    public required int WaveNumber { get; init; }
    public required int TotalWaves { get; init; }

    public required IReadOnlyList<WaveEntry> Entries { get; init; }
}