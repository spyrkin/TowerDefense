

/// <summary>
/// Состояние башни. Инкапсулирует инварианты уровня, экономики и доступа к конфигурации.
/// </summary>
public sealed class TowerState
{
    private int _level = MechanicsLimits.MinUpgradeLevel;

    public required Guid Id { get; init; }
    public required TowerType Type { get; init; }
    
    // Позиция башни фиксируется при установке. 
    // Если в игре не предусмотрено перемещение башен, рекомендуется заменить set на init.
    public required Point2D Position { get; set; } 

    /// <summary>
    /// Текущий уровень башни. 
    /// Жестко ограничен глобальными лимитами и фактическим количеством уровней в TowerType.
    /// </summary>
    public int Level
    {
        get => _level;
        private set
        {
            int maxAllowedLevel = Math.Min(MechanicsLimits.MaxUpgradeLevel, Type.Levels.Count);
            if (value < MechanicsLimits.MinUpgradeLevel || value > maxAllowedLevel)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value), 
                    $"Level must be between {MechanicsLimits.MinUpgradeLevel} and {maxAllowedLevel}.");
            }
            _level = value;
        }
    }

    /// <summary>
    /// Суммарное золото, вложенное в башню (стоимость покупки + сумма апгрейдов).
    /// Используется для расчета стоимости продажи.
    /// </summary>
    public int InvestedGold { get; private set; }

    /// <summary>
    /// Быстрый доступ к конфигурации текущего уровня (урон, радиус, кулдаун и т.д.).
    /// </summary>
    public TowerLevelDefinition CurrentLevelDefinition => Type.Levels[Level - 1];

    public bool CanUpgrade => Level < Math.Min(MechanicsLimits.MaxUpgradeLevel, Type.Levels.Count);

    /// <summary>
    /// Инициализирует базовую стоимость при покупке башни.
    /// </summary>
    public void InitializeInvestment(int baseCost)
    {
        if (InvestedGold != 0) 
            throw new InvalidOperationException("Investment has already been initialized.");
        if (baseCost < 0) 
            throw new ArgumentException("Base cost cannot be negative.");
            
        InvestedGold = baseCost;
    }

    /// <summary>
    /// Рассчитывает стоимость следующего апгрейда.
    /// </summary>
    public int GetUpgradeCost()
    {
        if (!CanUpgrade) return 0;
        // Level - 1 это текущий уровень (индекс). Level это индекс следующего уровня.
        return Type.Levels[Level].UpgradeCost; 
    }

    /// <summary>
    /// Применяет апгрейд башни. 
    /// Примечание: Проверка наличия золота у игрока должна выполняться в Domain Service до вызова этого метода.
    /// </summary>
    public void Upgrade()
    {
        if (!CanUpgrade)
            throw new InvalidOperationException("Tower is already at maximum level.");

        InvestedGold += GetUpgradeCost();
        Level++;
    }
}