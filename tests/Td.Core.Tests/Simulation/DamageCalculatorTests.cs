using System;

public sealed class DamageCalculatorTests
{
    private readonly DamageCalculator _calculator = new();

    [Fact]
    public void CalculateDamage_ReturnsAtLeastOne()
    {
        var towerLevel = CreateTowerLevel(damage: 5, armorIgnorePercent: 0.0);
        var monsterType = CreateMonsterType(armor: 20);

        int damage = _calculator.CalculateDamage(towerLevel, monsterType);

        Assert.Equal(1, damage);
    }

    [Fact]
    public void CalculateDamage_AppliesFlatReduction()
    {
        var towerLevel = CreateTowerLevel(damage: 20, armorIgnorePercent: 0.0);
        var monsterType = CreateMonsterType(armor: 5);

        int damage = _calculator.CalculateDamage(towerLevel, monsterType);

        Assert.Equal(15, damage);
    }

    [Fact]
    public void CalculateDamage_AppliesPartialArmorIgnore()
    {
        var towerLevel = CreateTowerLevel(damage: 20, armorIgnorePercent: 0.5);
        var monsterType = CreateMonsterType(armor: 10);

        int damage = _calculator.CalculateDamage(towerLevel, monsterType);

        // EffectiveArmor = Floor(10 * 0.5) = 5
        Assert.Equal(15, damage);
    }

    [Fact]
    public void CalculateDamage_AppliesFullArmorIgnore()
    {
        var towerLevel = CreateTowerLevel(damage: 20, armorIgnorePercent: 1.0);
        var monsterType = CreateMonsterType(armor: 10);

        int damage = _calculator.CalculateDamage(towerLevel, monsterType);

        Assert.Equal(20, damage);
    }

    [Fact]
    public void CalculateDamage_ClampsNegativeArmorIgnoreToZero()
    {
        var towerLevel = CreateTowerLevel(damage: 20, armorIgnorePercent: -0.5);
        var monsterType = CreateMonsterType(armor: 10);

        int damage = _calculator.CalculateDamage(towerLevel, monsterType);

        // ArmorIgnorePercent clamped to 0, EffectiveArmor = 10
        Assert.Equal(10, damage);
    }

    [Fact]
    public void CalculateDamage_ClampsArmorIgnoreAboveOneToOne()
    {
        var towerLevel = CreateTowerLevel(damage: 20, armorIgnorePercent: 1.5);
        var monsterType = CreateMonsterType(armor: 10);

        int damage = _calculator.CalculateDamage(towerLevel, monsterType);

        // ArmorIgnorePercent clamped to 1, EffectiveArmor = 0
        Assert.Equal(20, damage);
    }

    [Fact]
    public void CalculateDamage_UsesFloorForEffectiveArmor()
    {
        var towerLevel = CreateTowerLevel(damage: 20, armorIgnorePercent: 0.33);
        var monsterType = CreateMonsterType(armor: 3);

        int damage = _calculator.CalculateDamage(towerLevel, monsterType);

        // EffectiveArmor = Floor(3 * 0.67) = Floor(2.01) = 2
        Assert.Equal(18, damage);
    }

    private static TowerLevelDefinition CreateTowerLevel(
        int damage,
        double armorIgnorePercent)
    {
        return new TowerLevelDefinition
        {
            Level = 1,
            UpgradeCost = 0,
            Damage = damage,
            Range = 100.0,
            CooldownSeconds = 1.0,
            SlowPercent = 0.0,
            SlowDurationSeconds = 0.0,
            SplashRadius = 0.0,
            ArmorIgnorePercent = armorIgnorePercent
        };
    }

    private static MonsterType CreateMonsterType(int armor)
    {
        return new MonsterType
        {
            Id = "monster_test",
            Name = "Test Monster",
            Class = MonsterClass.Normal,
            MaxHp = 100,
            Speed = 50.0,
            Armor = armor,
            GoldReward = 10,
            SlowResistance = 0.0,
            Tier = 1
        };
    }

}