

/// <summary>
/// Состояние монстра. Инкапсулирует инварианты здоровья, прогресса по пути и статусов.
/// </summary>
public sealed class MonsterState
{
    private double _pathProgress;
    private double _currentHp;

    public required Guid Id { get; init; }
    public required MonsterType Type { get; init; }
    
    /// <summary>
    /// Текущее здоровье. Жестко ограничено диапазоном [0, Type.MaxHp] для предотвращения оверхила.
    /// </summary>
    public double CurrentHp 
    { 
        get => _currentHp; 
        set => _currentHp = Math.Clamp(value, 0.0, Type.MaxHp); 
    }

    public required Point2D Position { get; set; }

    /// <summary>
    /// Нормализованный прогресс монстра по пути от 0.0 (спавн) до 1.0 (финиш/база).
    /// </summary>
    public double PathProgress 
    { 
        get => _pathProgress; 
        set => _pathProgress = Math.Clamp(value, 0.0, 1.0); 
    }

    public SlowStatus? Slow { get; private set; }

    public bool IsAlive => CurrentHp > 0;

    /// <summary>
    /// Применяет эффект замедления. 
    /// Учитывает сопротивление монстра и абсолютный лимит игры (MaxSlowPercent).
    /// Правило стакинга: берется максимальный процент, таймер обновляется до максимального значения.
    /// </summary>
	public void ApplySlow(double basePercent, double durationSeconds)
	{
		if (basePercent <= 0 || durationSeconds <= 0) return;

		double clampedBasePercent = Math.Clamp(basePercent, 0.0, 1.0);
		double clampedSlowResistance = Math.Clamp(Type.SlowResistance, 0.0, 1.0);

		// 1. Учет сопротивления (линейное снижение)
		double effectivePercent = clampedBasePercent * (1.0 - clampedSlowResistance);

		// 2. Применение абсолютного лимита игры (потолок)
		effectivePercent = Math.Min(effectivePercent, MechanicsLimits.MaxSlowPercent);

		// 3. Стакинг (Refresh & Max)
		if (Slow == null)
		{
			Slow = new SlowStatus
			{
				Percent = effectivePercent,
				RemainingSeconds = durationSeconds
			};
		}
		else
		{
			Slow = Slow with
			{
				Percent = Math.Max(Slow.Percent, effectivePercent),
				RemainingSeconds = Math.Max(Slow.RemainingSeconds, durationSeconds)
			};
		}
	}

    /// <summary>
    /// Обновляет таймер замедления на шаг симуляции. Сбрасывает эффект при истечении.
    /// </summary>
    public void TickSlow(double deltaSeconds)
    {
        if (Slow == null) return;

        double newTime = Slow.RemainingSeconds - deltaSeconds;
        if (newTime <= 0)
        {
            Slow = null;
        }
        else
        {
            Slow = Slow with { RemainingSeconds = newTime };
        }
    }
}