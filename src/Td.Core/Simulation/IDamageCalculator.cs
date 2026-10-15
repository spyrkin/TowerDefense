

/// <summary>
/// Калькулятор урона. Инкапсулирует математику взаимодействия брони и игнора брони.
/// </summary>
public interface IDamageCalculator
{
    /// <summary>
    /// Рассчитывает итоговый урон по монстру с учетом его брони и процента игнора брони башни.
    /// </summary>
    /// <param name="towerLevel">Текущий уровень башни (содержит Damage и ArmorIgnorePercent).</param>
    /// <param name="monsterType">Тип монстра (содержит Armor).</param>
    /// <returns>
    /// Итоговый урон (целое число, не менее 1).
    /// Строгая формула расчета (Flat Reduction Model):
    /// 1. EffectiveArmor = Floor(MonsterType.Armor * (1 - TowerLevelDefinition.ArmorIgnorePercent))
    /// 2. FinalDamage = Max(1, TowerLevelDefinition.Damage - EffectiveArmor)
    /// </returns>
    int CalculateDamage(TowerLevelDefinition towerLevel, MonsterType monsterType);
}