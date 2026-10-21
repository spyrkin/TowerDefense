# ADR 0004: Random wave generation assumptions

## Статус
Принято / Предложено к подтверждению

## Контекст
Для реализации `N7 Random wave generator` недостаточно одного интерфейса `IRandomWaveGenerator`.
Необходимо зафиксировать правила:
- выбора общего количества волн;
- бюджета волны;
- максимально допустимого уровня монстров;
- лимита монстров в волне;
- детерминированности при использовании `System.Random`.

## Решение

### 1. Количество волн
Общее количество волн выбирается случайно:

```text
[WaveGenerationRules.MinWaveCount, WaveGenerationRules.MaxWaveCount]