public sealed record GameSettings
{
    public required bool SoundEnabled { get; init; }

    public required double Volume { get; init; }

    public required GameSpeed DefaultSpeed { get; init; }

    public required bool ShowTowerRange { get; init; }
}