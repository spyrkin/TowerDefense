using System;

/// <summary>
/// Реализация калькулятора урона по модели Flat Reduction.
/// </summary>
public sealed class DamageCalculator : IDamageCalculator
{
    public int CalculateDamage(TowerLevelDefinition towerLevel, MonsterType monsterType)
    {
        ArgumentNullException.ThrowIfNull(towerLevel);
        ArgumentNullException.ThrowIfNull(monsterType);

        double armorIgnorePercent = Math.Clamp(towerLevel.ArmorIgnorePercent, 0.0, 1.0);

        int effectiveArmor = (int)Math.Floor(monsterType.Armor * (1.0 - armorIgnorePercent));

        return Math.Max(1, towerLevel.Damage - effectiveArmor);
    }
}