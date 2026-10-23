using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Генератор случайных волн.
/// Правила генерации зафиксированы в ADR 0004.
/// Баланс является плейсхолдером до этапа N15.
/// </summary>
public sealed class RandomWaveGenerator : IRandomWaveGenerator
{
    private const int SafetyIterationLimit = 10_000;

    private readonly IReadOnlyList<MonsterType> _monsterCatalog;

    public RandomWaveGenerator(IReadOnlyList<MonsterType> monsterCatalog)
    {
        ArgumentNullException.ThrowIfNull(monsterCatalog);

        if (monsterCatalog.Count == 0)
        {
            throw new ArgumentException("Monster catalog cannot be empty.", nameof(monsterCatalog));
        }

        _monsterCatalog = monsterCatalog;
    }

    public int RollTotalWaveCount(Random random)
    {
        ArgumentNullException.ThrowIfNull(random);

        return random.Next(
            WaveGenerationRules.MinWaveCount,
            WaveGenerationRules.MaxWaveCount + 1);
    }

    public WaveDefinition GenerateWave(int waveNumber, int totalWaves, Random random)
    {
        ArgumentNullException.ThrowIfNull(random);
        ValidateWaveNumbers(waveNumber, totalWaves);

        int maxTier = GetMaxAllowedTier(waveNumber, totalWaves);
        int budget = GetWaveBudget(waveNumber);
        int maxMonsters = Math.Max(1, WaveGenerationRules.MaxAliveMonsters);

        List<MonsterType> allowed = _monsterCatalog
            .Where(monster => monster.Tier >= WaveGenerationRules.MinMonsterTier)
            .Where(monster => monster.Tier <= maxTier)
            .OrderBy(monster => monster.Id, StringComparer.Ordinal)
            .ToList();

        if (allowed.Count == 0)
        {
            throw new InvalidOperationException(
                $"No monsters are available for wave {waveNumber}/{totalWaves} with max tier {maxTier}.");
        }

        Dictionary<string, int> counts = new(StringComparer.Ordinal);
        int totalMonsters = 0;

        TryAddGuaranteedMaxTierMonster(
            allowed,
            maxTier,
            random,
            ref budget,
            ref totalMonsters,
            maxMonsters,
            counts);

        int minCost = allowed.Min(MonsterCost);
        int guard = 0;

        while (budget >= minCost && totalMonsters < maxMonsters && guard < SafetyIterationLimit)
        {
            guard++;

            MonsterType candidate = allowed[random.Next(allowed.Count)];
            int cost = MonsterCost(candidate);

            if (cost > budget)
            {
                continue;
            }

            AddMonster(counts, candidate.Id);
            budget -= cost;
            totalMonsters++;
        }

        if (totalMonsters == 0)
        {
            MonsterType cheapest = allowed
                .OrderBy(monster => MonsterCost(monster))
                .ThenBy(monster => monster.Id, StringComparer.Ordinal)
                .First();

            AddMonster(counts, cheapest.Id);
            totalMonsters = 1;
        }

        IReadOnlyList<WaveEntry> entries = counts
            .OrderBy(pair => pair.Key, StringComparer.Ordinal)
            .Select(pair => new WaveEntry
            {
                MonsterTypeId = pair.Key,
                Count = pair.Value
            })
            .ToList();

        return new WaveDefinition
        {
            WaveNumber = waveNumber,
            TotalWaves = totalWaves,
            Entries = entries
        };
    }

    public IReadOnlyList<WaveDefinition> GenerateAllWaves(Random random)
    {
        ArgumentNullException.ThrowIfNull(random);

        int totalWaves = RollTotalWaveCount(random);
        List<WaveDefinition> waves = new(totalWaves);

        for (int waveNumber = 1; waveNumber <= totalWaves; waveNumber++)
        {
            waves.Add(GenerateWave(waveNumber, totalWaves, random));
        }

        return waves;
    }

    private static void ValidateWaveNumbers(int waveNumber, int totalWaves)
    {
        if (totalWaves < 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(totalWaves),
                "Total waves must be positive.");
        }

        if (waveNumber < 1 || waveNumber > totalWaves)
        {
            throw new ArgumentOutOfRangeException(
                nameof(waveNumber),
                "Wave number must be between 1 and total waves.");
        }
    }

    private static int GetWaveBudget(int waveNumber)
    {
        return WaveGenerationRules.BaseWaveBudget +
               (waveNumber - 1) * WaveGenerationRules.WaveBudgetGrowth;
    }

    private static int GetMaxAllowedTier(int waveNumber, int totalWaves)
    {
        int tierSpan = WaveGenerationRules.MaxMonsterTier - WaveGenerationRules.MinMonsterTier;

        if (tierSpan <= 0)
        {
            return WaveGenerationRules.MinMonsterTier;
        }

        double progress = (waveNumber - 1) / (double)Math.Max(1, totalWaves - 1);
        int tier = WaveGenerationRules.MinMonsterTier + (int)Math.Floor(progress * tierSpan);

        return Math.Clamp(
            tier,
            WaveGenerationRules.MinMonsterTier,
            WaveGenerationRules.MaxMonsterTier);
    }

    private static int MonsterCost(MonsterType monster)
    {
        return Math.Max(1, monster.Tier);
    }

    private static void TryAddGuaranteedMaxTierMonster(
        IReadOnlyList<MonsterType> allowed,
        int maxTier,
        Random random,
        ref int budget,
        ref int totalMonsters,
        int maxMonsters,
        IDictionary<string, int> counts)
    {
        if (totalMonsters >= maxMonsters)
        {
            return;
        }

        List<MonsterType> candidates = allowed
            .Where(monster => monster.Tier == maxTier)
            .ToList();

        if (candidates.Count == 0)
        {
            return;
        }

        MonsterType monster = candidates[random.Next(candidates.Count)];
        int cost = MonsterCost(monster);

        if (cost > budget)
        {
            return;
        }

        AddMonster(counts, monster.Id);
        budget -= cost;
        totalMonsters++;
    }

    private static void AddMonster(IDictionary<string, int> counts, string monsterTypeId)
    {
        counts[monsterTypeId] = counts.TryGetValue(monsterTypeId, out int existing)
            ? existing + 1
            : 1;
    }
}