using System.Collections.Generic;

public static class TowerContent
{
    public static IReadOnlyList<TowerType> All { get; } = new List<TowerType>
    {
        new TowerType
        {
            Id = "tower_gun",
            Name = "Gun Tower",
            Kind = TowerKind.Normal,
            BaseCost = 100,
            Levels = new List<TowerLevelDefinition>
            {
                new TowerLevelDefinition
                {
                    Level = 1,
                    UpgradeCost = 0,
                    Damage = 12,
                    Range = 120.0,
                    CooldownSeconds = 0.60,
                    SlowPercent = 0.0,
                    SlowDurationSeconds = 0.0,
                    SplashRadius = 0.0,
                    ArmorIgnorePercent = 0.0
                },

                new TowerLevelDefinition
                {
                    Level = 2,
                    UpgradeCost = 80,
                    Damage = 20,
                    Range = 130.0,
                    CooldownSeconds = 0.55,
                    SlowPercent = 0.0,
                    SlowDurationSeconds = 0.0,
                    SplashRadius = 0.0,
                    ArmorIgnorePercent = 0.0
                },

                new TowerLevelDefinition
                {
                    Level = 3,
                    UpgradeCost = 130,
                    Damage = 32,
                    Range = 140.0,
                    CooldownSeconds = 0.50,
                    SlowPercent = 0.0,
                    SlowDurationSeconds = 0.0,
                    SplashRadius = 0.0,
                    ArmorIgnorePercent = 0.0
                }
            }
        },

        new TowerType
        {
            Id = "tower_frost",
            Name = "Frost Tower",
            Kind = TowerKind.Slow,
            BaseCost = 120,
            Levels = new List<TowerLevelDefinition>
            {
                new TowerLevelDefinition
                {
                    Level = 1,
                    UpgradeCost = 0,
                    Damage = 6,
                    Range = 110.0,
                    CooldownSeconds = 0.80,
                    SlowPercent = 0.25,
                    SlowDurationSeconds = 1.5,
                    SplashRadius = 0.0,
                    ArmorIgnorePercent = 0.0
                },

                new TowerLevelDefinition
                {
                    Level = 2,
                    UpgradeCost = 90,
                    Damage = 9,
                    Range = 120.0,
                    CooldownSeconds = 0.75,
                    SlowPercent = 0.35,
                    SlowDurationSeconds = 2.0,
                    SplashRadius = 0.0,
                    ArmorIgnorePercent = 0.0
                },

                new TowerLevelDefinition
                {
                    Level = 3,
                    UpgradeCost = 140,
                    Damage = 12,
                    Range = 130.0,
                    CooldownSeconds = 0.70,
                    SlowPercent = 0.45,
                    SlowDurationSeconds = 2.5,
                    SplashRadius = 0.0,
                    ArmorIgnorePercent = 0.0
                }
            }
        },

        new TowerType
        {
            Id = "tower_cannon",
            Name = "Cannon Tower",
            Kind = TowerKind.Area,
            BaseCost = 160,
            Levels = new List<TowerLevelDefinition>
            {
                new TowerLevelDefinition
                {
                    Level = 1,
                    UpgradeCost = 0,
                    Damage = 18,
                    Range = 130.0,
                    CooldownSeconds = 1.40,
                    SlowPercent = 0.0,
                    SlowDurationSeconds = 0.0,
                    SplashRadius = 45.0,
                    ArmorIgnorePercent = 0.0
                },

                new TowerLevelDefinition
                {
                    Level = 2,
                    UpgradeCost = 120,
                    Damage = 28,
                    Range = 140.0,
                    CooldownSeconds = 1.30,
                    SlowPercent = 0.0,
                    SlowDurationSeconds = 0.0,
                    SplashRadius = 55.0,
                    ArmorIgnorePercent = 0.0
                },

                new TowerLevelDefinition
                {
                    Level = 3,
                    UpgradeCost = 180,
                    Damage = 40,
                    Range = 150.0,
                    CooldownSeconds = 1.20,
                    SlowPercent = 0.0,
                    SlowDurationSeconds = 0.0,
                    SplashRadius = 65.0,
                    ArmorIgnorePercent = 0.0
                }
            }
        },

        new TowerType
        {
            Id = "tower_sniper",
            Name = "Sniper Tower",
            Kind = TowerKind.ArmorPiercing,
            BaseCost = 180,
            Levels = new List<TowerLevelDefinition>
            {
                new TowerLevelDefinition
                {
                    Level = 1,
                    UpgradeCost = 0,
                    Damage = 25,
                    Range = 170.0,
                    CooldownSeconds = 1.60,
                    SlowPercent = 0.0,
                    SlowDurationSeconds = 0.0,
                    SplashRadius = 0.0,
                    ArmorIgnorePercent = 0.45
                },

                new TowerLevelDefinition
                {
                    Level = 2,
                    UpgradeCost = 140,
                    Damage = 40,
                    Range = 190.0,
                    CooldownSeconds = 1.50,
                    SlowPercent = 0.0,
                    SlowDurationSeconds = 0.0,
                    SplashRadius = 0.0,
                    ArmorIgnorePercent = 0.65
                },

                new TowerLevelDefinition
                {
                    Level = 3,
                    UpgradeCost = 200,
                    Damage = 60,
                    Range = 210.0,
                    CooldownSeconds = 1.40,
                    SlowPercent = 0.0,
                    SlowDurationSeconds = 0.0,
                    SplashRadius = 0.0,
                    ArmorIgnorePercent = 0.85
                }
            }
        }
    };
}