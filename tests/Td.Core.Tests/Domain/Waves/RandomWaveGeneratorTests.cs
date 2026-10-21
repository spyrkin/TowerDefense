using System;
using System.Collections.Generic;
using System.Linq;

public sealed class RandomWaveGeneratorTests
{
    private readonly RandomWaveGenerator _generator = new(MonsterContent.All);

    [Fact]
    public void RollTotalWaveCount_ReturnsValueInRange()
    {
        for (int seed = 0; seed < 100; seed++)
        {
            Random random = new(seed);

            int totalWaves = _generator.RollTotalWaveCount(random);

            Assert.InRange(
                totalWaves,
                WaveGenerationRules.MinWaveCount,
                WaveGenerationRules.MaxWaveCount);
        }
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(11, 10)]
    [InlineData(1, 0)]
    public void GenerateWave_ThrowsForInvalidArguments(int waveNumber, int totalWaves)
    {
        Random random = new(42);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => _generator.GenerateWave(waveNumber, totalWaves, random));
    }

    [Fact]
    public void GenerateWave_ReturnsValidWave()
    {
        Random random = new(42);

        WaveDefinition wave = _generator.GenerateWave(5, 20, random);

        Assert.Equal(5, wave.WaveNumber);
        Assert.Equal(20, wave.TotalWaves);
        Assert.NotEmpty(wave.Entries);

        int totalMonsters = 0;

        foreach (WaveEntry entry in wave.Entries)
        {
            Assert.False(string.IsNullOrWhiteSpace(entry.MonsterTypeId));
            Assert.True(entry.Count > 0);

            MonsterType? monster = MonsterContent.All
                .FirstOrDefault(m => m.Id == entry.MonsterTypeId);

            Assert.NotNull(monster);

            totalMonsters += entry.Count;
        }

        Assert.True(totalMonsters <= WaveGenerationRules.MaxAliveMonsters);
    }

    [Fact]
    public void GenerateWave_FirstWave_UsesOnlyTierOneMonsters()
    {
        Random random = new(42);

        WaveDefinition wave = _generator.GenerateWave(1, 20, random);

        foreach (WaveEntry entry in wave.Entries)
        {
            MonsterType monster = MonsterContent.All
                .First(m => m.Id == entry.MonsterTypeId);

            Assert.Equal(1, monster.Tier);
        }
    }

    [Fact]
    public void GenerateWave_IsDeterministicForSameSeed()
    {
        WaveDefinition first = _generator.GenerateWave(7, 20, new Random(123));
        WaveDefinition second = _generator.GenerateWave(7, 20, new Random(123));

        Assert.Equal(first.WaveNumber, second.WaveNumber);
        Assert.Equal(first.TotalWaves, second.TotalWaves);
        Assert.Equal(first.Entries.Count, second.Entries.Count);

        for (int i = 0; i < first.Entries.Count; i++)
        {
            Assert.Equal(first.Entries[i].MonsterTypeId, second.Entries[i].MonsterTypeId);
            Assert.Equal(first.Entries[i].Count, second.Entries[i].Count);
        }
    }

    [Fact]
    public void GenerateAllWaves_ReturnsSequentialWaves()
    {
        Random random = new(77);

        IReadOnlyList<WaveDefinition> waves = _generator.GenerateAllWaves(random);

        Assert.InRange(
            waves.Count,
            WaveGenerationRules.MinWaveCount,
            WaveGenerationRules.MaxWaveCount);

        for (int i = 0; i < waves.Count; i++)
        {
            Assert.Equal(i + 1, waves[i].WaveNumber);
            Assert.Equal(waves.Count, waves[i].TotalWaves);
            Assert.NotEmpty(waves[i].Entries);
        }
    }
}