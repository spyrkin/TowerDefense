using System.Collections.Generic;

public sealed class TowerTypeTargetingTests
{
    [Fact]
    public void CanTargetFlying_DefaultsToTrue()
    {
        var towerType = CreateTowerType();

        Assert.True(towerType.CanTargetFlying);
    }

    [Fact]
    public void CanTargetFlying_CanBeSetToFalse()
    {
        var towerType = CreateTowerType(canTargetFlying: false);

        Assert.False(towerType.CanTargetFlying);
    }

    private static TowerType CreateTowerType(bool canTargetFlying = true)
    {
        return new TowerType
        {
            Id = "tower_test",
            Name = "Test Tower",
            Kind = TowerKind.Normal,
            BaseCost = 100,
            Levels = new List<TowerLevelDefinition>
            {
                new TowerLevelDefinition
                {
                    Level = 1,
                    UpgradeCost = 0,
                    Damage = 10,
                    Range = 100.0,
                    CooldownSeconds = 1.0,
                    SlowPercent = 0.0,
                    SlowDurationSeconds = 0.0,
                    SplashRadius = 0.0,
                    ArmorIgnorePercent = 0.0
                }
            },
            CanTargetFlying = canTargetFlying
        };
    }
}