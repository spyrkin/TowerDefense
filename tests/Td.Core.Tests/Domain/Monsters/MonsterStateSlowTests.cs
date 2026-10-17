using System;

public sealed class MonsterStateSlowTests
{
    [Fact]
    public void ApplySlow_AppliesEffectivePercentAfterResistance()
    {
        var monster = CreateMonster(slowResistance: 0.2);

        monster.ApplySlow(basePercent: 0.5, durationSeconds: 2.0);

        Assert.NotNull(monster.Slow);
        Assert.Equal(0.4, monster.Slow.Percent);
        Assert.Equal(2.0, monster.Slow.RemainingSeconds);
    }

    [Fact]
    public void ApplySlow_CapsAtMaxSlowPercent()
    {
        var monster = CreateMonster(slowResistance: 0.0);

        monster.ApplySlow(basePercent: 1.0, durationSeconds: 2.0);

        Assert.NotNull(monster.Slow);
        Assert.Equal(MechanicsLimits.MaxSlowPercent, monster.Slow.Percent);
    }

    [Fact]
    public void ApplySlow_KeepsMaxPercentAndMaxDuration()
    {
        var monster = CreateMonster(slowResistance: 0.0);

        monster.ApplySlow(basePercent: 0.3, durationSeconds: 2.0);
        monster.ApplySlow(basePercent: 0.5, durationSeconds: 1.0);

        Assert.NotNull(monster.Slow);
        Assert.Equal(0.5, monster.Slow.Percent);
        Assert.Equal(2.0, monster.Slow.RemainingSeconds);
    }

    [Fact]
    public void ApplySlow_RefreshesDurationIfIncomingDurationIsLonger()
    {
        var monster = CreateMonster(slowResistance: 0.0);

        monster.ApplySlow(basePercent: 0.5, durationSeconds: 1.0);
        monster.ApplySlow(basePercent: 0.3, durationSeconds: 3.0);

        Assert.NotNull(monster.Slow);
        Assert.Equal(0.5, monster.Slow.Percent);
        Assert.Equal(3.0, monster.Slow.RemainingSeconds);
    }

    [Fact]
    public void ApplySlow_IgnoresZeroDuration()
    {
        var monster = CreateMonster(slowResistance: 0.0);

        monster.ApplySlow(basePercent: 0.5, durationSeconds: 0.0);

        Assert.Null(monster.Slow);
    }

    [Fact]
    public void ApplySlow_IgnoresZeroPercent()
    {
        var monster = CreateMonster(slowResistance: 0.0);

        monster.ApplySlow(basePercent: 0.0, durationSeconds: 2.0);

        Assert.Null(monster.Slow);
    }

    [Fact]
    public void TickSlow_DecreasesRemainingSeconds()
    {
        var monster = CreateMonster(slowResistance: 0.0);

        monster.ApplySlow(basePercent: 0.4, durationSeconds: 2.0);
        monster.TickSlow(deltaSeconds: 0.5);

        Assert.NotNull(monster.Slow);
        Assert.Equal(1.5, monster.Slow.RemainingSeconds);
    }

    [Fact]
    public void TickSlow_RemovesSlowWhenExpired()
    {
        var monster = CreateMonster(slowResistance: 0.0);

        monster.ApplySlow(basePercent: 0.4, durationSeconds: 1.0);
        monster.TickSlow(deltaSeconds: 1.0);

        Assert.Null(monster.Slow);
    }

    private static MonsterState CreateMonster(double slowResistance)
    {
        var type = new MonsterType
        {
            Id = "monster_test",
            Name = "Test Monster",
            Class = MonsterClass.Normal,
            MaxHp = 100,
            Speed = 50.0,
            Armor = 0,
            GoldReward = 10,
            SlowResistance = slowResistance,
            Tier = 1
        };

        return new MonsterState
        {
            Id = Guid.NewGuid(),
            Type = type,
            Position = Point2D.Zero,
            CurrentHp = type.MaxHp
        };
    }
}