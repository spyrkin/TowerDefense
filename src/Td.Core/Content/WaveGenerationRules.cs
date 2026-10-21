public static class WaveGenerationRules
{
    public const int MinWaveCount = 3;
    public const int MaxWaveCount = 20;
    public const int MinMonsterTier = 1;
    public const int MaxMonsterTier = 5;
    public const int MaxAliveMonsters = 20;

    /// <summary>
    /// Базовый бюджет первой волны.
    /// Плейсхолдер до балансировки на этапе N15.
    /// </summary>
    public const int BaseWaveBudget = 8;

    /// <summary>
    /// Прирост бюджета волны за каждый следующий номер волны.
    /// Плейсхолдер до балансировки на этапе N15.
    /// </summary>
    public const int WaveBudgetGrowth = 4;

    /// <summary>
    /// Интервал спавна монстров внутри волны.
    /// Плейсхолдер до балансировки.
    /// </summary>
    public const double SpawnIntervalSeconds = 0.75;
}