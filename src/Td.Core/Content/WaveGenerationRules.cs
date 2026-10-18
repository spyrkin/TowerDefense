public static class WaveGenerationRules
{
    public const int MinWaveCount = 3;
    public const int MaxWaveCount = 20;
    public const int MinMonsterTier = 1;
    public const int MaxMonsterTier = 5;
    public const int MaxAliveMonsters = 20;

    /// <summary>
    /// Интервал спавна монстров внутри волны.
    /// Плейсхолдер до балансировки.
    /// </summary>
    public const double SpawnIntervalSeconds = 0.75;
}