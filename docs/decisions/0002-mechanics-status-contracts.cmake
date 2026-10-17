# ADR 0002: Mechanics/status contracts

## Статус

Принято

## Контекст

Для перехода к `N6 Simulation engine` необходимо однозначно зафиксировать правила:

- урона;
- брони;
- игнора брони;
- бронебойности;
- замедления;
- сопротивления замедлению;
- летающих целей;
- площадного урона;
- статусов;
- времени действия эффектов.

## Решение

### 1. Модель урона

Принята `Flat Reduction Model`.

Формула:

```text
EffectiveArmor = Floor(MonsterType.Armor * (1 - TowerLevelDefinition.ArmorIgnorePercent))
FinalDamage = Max(1, TowerLevelDefinition.Damage - EffectiveArmor)
```

Итоговый урон всегда не меньше `1`.

### 2. ArmorIgnorePercent

`ArmorIgnorePercent` трактуется как доля и ограничивается диапазоном:

```text
[0.0, 1.0]
```

Правила:

```text
ArmorIgnorePercent < 0.0 -> 0.0
ArmorIgnorePercent > 1.0 -> 1.0
```

При `ArmorIgnorePercent = 1.0` броня игнорируется полностью, но минимальный урон все равно равен `1`.

### 3. ArmorPiercing

`TowerKind.ArmorPiercing` является семантическим типом башни.

Механика бронебойности реализуется только через:

```text
TowerLevelDefinition.ArmorIgnorePercent
```

Отдельная формула для `TowerKind.ArmorPiercing` не вводится.

### 4. Splash / площадной урон

Если:

```text
TowerLevelDefinition.SplashRadius > 0
```

то урон получают все монстры в радиусе `SplashRadius` от точки попадания.

Правила:

```text
- урон рассчитывается индивидуально для каждого монстра;
- броня и ArmorIgnorePercent применяются к каждому монстру отдельно;
- затухание урона по расстоянию отсутствует;
- splash-урон не наносит урон башням и базе;
- SplashRadius <= 0 означает отсутствие площадного урона.
```

Конкретная механика выбора целей и обработки попадания реализуется в `N6 Simulation engine`.

### 5. Летающие цели

`MonsterClass.Flying` означает воздушную цель.

Вводится флаг:

```csharp
public bool CanTargetFlying { get; init; } = true;
```

в `TowerType`.

Правило:

```text
Если MonsterType.Class == Flying,
башня может атаковать этого монстра только если TowerType.CanTargetFlying == true.
```

Значение по умолчанию:

```text
true
```

Это сохраняет обратную совместимость и позволяет позже точечно отключать атаку воздушных целей у отдельных башен.

### 6. Замедление

Замедление применяется, если:

```text
TowerLevelDefinition.SlowPercent > 0
```

Независимо от `TowerKind`.

Значения:

```text
SlowPercent
SlowResistance
```

трактуются как доли в диапазоне:

```text
[0.0, 1.0]
```

Например:

```text
0.25 = 25%
0.70 = 70%
```

Формула применения:

```text
effectivePercent = basePercent * (1 - SlowResistance)
effectivePercent = min(effectivePercent, MechanicsLimits.MaxSlowPercent)
```

Правило стакинга:

```text
Percent = max(existing.Percent, incomingEffectivePercent)
RemainingSeconds = max(existing.RemainingSeconds, incomingDurationSeconds)
```

### 7. Статусы

В первой версии поддерживается только один статус:

```text
SlowStatus
```

DoT, stun, knockback, shield, regen, burn, bleed и другие эффекты в `N5` не добавляются.

Универсальная статусная система пока не вводится.

### 8. Время эффектов

Все длительности эффектов измеряются в симуляционных секундах.

`GameSpeed` влияет на скорость накопления симуляционного времени в игровом цикле, но не меняет контракт статусов.

`SlowStatus.RemainingSeconds` уменьшается на `deltaSeconds`, переданный симуляцией.

## Последствия

- `N6 Simulation engine` получает однозначные правила урона, брони, замедления и летающих целей.
- Контент может управлять бронебойностью через `ArmorIgnorePercent`.
- Летающие цели можно будет балансировать через `TowerType.CanTargetFlying`.
- Статусная система остается минимальной и не расширяется до появления отдельных требований.