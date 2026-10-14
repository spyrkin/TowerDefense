# Tower Defense — DAG выполнения проекта

Дата последнего обновления: 2026-07-09  
Проект: `TowerDefense`  
Путь к решению: `C:\Users\Programmist\Desktop\Game`

---

## 1. Текущий статус

Выполнено по шаг `N3` включительно.

```text
DONE: N0, N1, N2, N3
NEXT: N4
```

Общий прогресс по узлам:

```text
4 / 19 узлов закрыто
```

Текущий активный шаг:

```text
N4 Content catalog
```

---

## 2. Зафиксированная основа проекта

Приняты допущения `A1-A21`.

Ключевые решения:

| Область | Решение |
|---|---|
| Платформа | Windows |
| Стек | `.NET 10`, WPF |
| Окно | фиксированное, `1280x720` |
| Карты | 3 карты в первой версии |
| Доступ к картам | все карты доступны сразу |
| Путь монстров | фиксированный |
| Размещение башен | свободное |
| Блокировка пути башнями | запрещена |
| Классы монстров | `Normal`, `Fast`, `Tank`, `Flying` |
| Boss | не используется |
| Волны | случайное количество от 3 до 20 |
| Рост сложности | через бюджет волны и уровень монстров |
| Скорость игры | `x1 / x2 / x4` |
| Клавиша скорости | `F` |
| Прокачка | линейная, 3 уровня |
| Продажа башен | разрешена |
| Жизни базы | 20 |
| Поражение | при 0 жизней |
| Настройки | звук, громкость, скорость по умолчанию, показ радиуса башни |
| Сохранение прогресса | нет |
| Сохранение настроек | локально |
| Анимация | процедурные заглушки |
| Звук | минимальные звуковые заглушки |
| Редакторы | вынесены в этап C |
| Автосборка | локальный скрипт; CI позже |

---

## 3. Легенда статусов

| Статус | Значение |
|---|---|
| `DONE` | шаг выполнен и принят |
| `ACTIVE` | шаг находится в работе |
| `PENDING` | шаг еще не начат |
| `BLOCKED` | шаг заблокирован |

---

## 4. Граф работ

```mermaid
graph TD
  N0["N0 Requirements v2 freeze"] --> N1["N1 Architecture/.NET10 skeleton"]
  N1 --> N2["N2 Domain contracts"]

  N0 --> N3["N3 Multiple map model"]
  N2 --> N3

  N0 --> N4["N4 Content catalog"]
  N2 --> N4

  N2 --> N5["N5 Mechanics/status contracts"]

  N2 --> N6["N6 Simulation engine"]
  N3 --> N6
  N4 --> N6
  N5 --> N6

  N4 --> N7["N7 Random wave generator"]
  N5 --> N7
  N6 --> N7

  N1 --> N8["N8 Rendering/animation shell"]
  N6 --> N8

  N6 --> N9["N9 Input/controller"]
  N8 --> N9

  N3 --> N10["N10 UI/HUD/menus/map select"]
  N6 --> N10
  N7 --> N10
  N8 --> N10
  N9 --> N10

  N1 --> N11["N11 Audio system"]
  N10 --> N11

  N1 --> N12["N12 Settings persistence"]
  N10 --> N12

  N3 --> N13["N13 Map editor MVP"]
  N10 --> N13

  N4 --> N14["N14 Wave editor MVP"]
  N7 --> N14
  N10 --> N14

  N4 --> N15["N15 Balance tuning"]
  N7 --> N15
  N10 --> N15

  N6 --> N16["N16 QA/perf/regression"]
  N7 --> N16
  N9 --> N16
  N10 --> N16
  N11 --> N16
  N12 --> N16
  N13 --> N16
  N14 --> N16
  N15 --> N16

  N1 --> N17["N17 CI/build automation"]
  N16 --> N17

  N16 --> N18["N18 Acceptance/release candidate"]
  N17 --> N18

  classDef done fill:#d1f7d1,stroke:#1e7e34,color:#000;
  classDef active fill:#fff3cd,stroke:#856404,color:#000;
  classDef pending fill:#eef1f4,stroke:#6c757d,color:#000;

  class N0,N1,N2,N3 done;
  class N4 active;
  class N5,N6,N7,N8,N9,N10,N11,N12,N13,N14,N15,N16,N17,N18 pending;
```

---

## 5. Таблица узлов

| ID | Шаг | Статус | Зависит от | Краткий результат |
|---|---|---|---|---|
| `N0` | Requirements v2 freeze | `DONE` | — | Требования заморожены, допущения `A1-A21` приняты |
| `N1` | Architecture/.NET10 skeleton | `DONE` | `N0` | Решение создано, проекты `Td.Core`, `Td.Wpf`, `Td.Core.Tests`, окно `1280x720`, базовый game loop |
| `N2` | Domain contracts | `DONE` | `N1` | Доменные контракты, интерфейсы, тесты инвариантов |
| `N3` | Multiple map model | `DONE` | `N0`, `N2` | Модель карт, геометрия пути, валидация размещения башен, 3 тестовые карты |
| `N4` | Content catalog | `ACTIVE` | `N0`, `N2` | 10 монстров, 4 башни, 3 уровня прокачки, экономика, правила волн |
| `N5` | Mechanics/status contracts | `PENDING` | `N2` | Контракты урона, брони, замедления, полета, статусов |
| `N6` | Simulation engine | `PENDING` | `N2`, `N3`, `N4`, `N5` | Движение, атака, урон, смерть, золото, жизни, победа/поражение |
| `N7` | Random wave generator | `PENDING` | `N4`, `N5`, `N6` | Случайные волны 3–20 с ростом сложности |
| `N8` | Rendering/animation shell | `PENDING` | `N1`, `N6` | WPF-отрисовка поля, монстров, башен, эффектов |
| `N9` | Input/controller | `PENDING` | `N6`, `N8` | Мышь и клавиатура, установка башен, апгрейд, продажа, пауза, скорость |
| `N10` | UI/HUD/menus/map select | `PENDING` | `N3`, `N6`, `N7`, `N8`, `N9` | HUD, меню, выбор карты, пауза, победа, поражение |
| `N11` | Audio system | `PENDING` | `N1`, `N10` | Минимальные звуковые события |
| `N12` | Settings persistence | `PENDING` | `N1`, `N10` | Локальное сохранение настроек |
| `N13` | Map editor MVP | `PENDING` | `N3`, `N10` | Редактор карт, этап C |
| `N14` | Wave editor MVP | `PENDING` | `N4`, `N7`, `N10` | Редактор/просмотр волн, этап C |
| `N15` | Balance tuning | `PENDING` | `N4`, `N7`, `N10` | Тюнинг экономики, монстров, башен, волн |
| `N16` | QA/perf/regression | `PENDING` | многие узлы | Тесты, производительность, регрессия |
| `N17` | CI/build automation | `PENDING` | `N1`, `N16` | Автосборка и прогон тестов |
| `N18` | Acceptance/release candidate | `PENDING` | `N16`, `N17` | Финальная приемка |

---

## 6. Детализация закрытых шагов

### `N0` — Requirements v2 freeze

Статус:

```text
DONE
```

Результат:

- приняты допущения `A1-A21`;
- зафиксированы требования к картам, монстрам, башням, волнам, управлению, настройкам;
- редакторы вынесены в этап C;
- согласован формат дальнейшей работы.

---

### `N1` — Architecture/.NET10 skeleton

Статус:

```text
DONE
```

Результат:

- создано решение;
- есть проекты:
  - `Td.Core`;
  - `Td.Wpf`;
  - `Td.Core.Tests`;
- WPF-окно фиксированного размера `1280x720`;
- сборка проходит;
- тесты проходят;
- зафиксировано решение по игровому циклу.

---

### `N2` — Domain contracts

Статус:

```text
DONE
```

Результат:

- добавлены доменные контракты;
- описаны сущности:
  - `Point2D`;
  - `MapDefinition`;
  - `MonsterType`;
  - `MonsterState`;
  - `TowerType`;
  - `TowerLevelDefinition`;
  - `TowerState`;
  - `WaveDefinition`;
  - `GameState`;
  - `GameSettings`;
- добавлены интерфейсы:
  - `IMapCatalog`;
  - `IPlacementValidator`;
  - `IRandomWaveGenerator`;
  - `IDamageCalculator`;
  - `ISellRefundPolicy`;
  - `ISettingsService`;
  - `IAudioService`;
- добавлены базовые ограничения:
  - волны от 3 до 20;
  - 3 уровня прокачки;
  - 20 жизней;
  - максимальное замедление 70%.

---

### `N3` — Multiple map model

Статус:

```text
DONE
```

Результат:

- реализована модель нескольких карт;
- добавлена геометрия пути;
- добавлена проверка размещения башен;
- созданы 3 тестовые карты;
- реализованы правила:
  - башня внутри игрового поля;
  - башня не размещается на пути;
  - башня не пересекается с другими башнями;
- добавлены тесты карт и валидации размещения.

---

## 7. Текущий активный шаг

### `N4` — Content catalog

Статус:

```text
ACTIVE
```

Цель:

- создать каталог игрового контента;
- добавить 10 типов монстров;
- добавить 4 типа башен;
- добавить 3 уровня прокачки для каждой башни;
- добавить экономику;
- добавить правила генерации волн;
- добавить тесты целостности контента.

Ожидаемые файлы:

```text
src/Td.Core/Content/ContentCatalog.cs
src/Td.Core/Content/MonsterContent.cs
src/Td.Core/Content/TowerContent.cs
src/Td.Core/Content/EconomyContent.cs
src/Td.Core/Content/WaveGenerationRules.cs
```

Ожидаемые тесты:

```text
tests/Td.Core.Tests/Content/MonsterContentTests.cs
tests/Td.Core.Tests/Content/TowerContentTests.cs
tests/Td.Core.Tests/Content/EconomyContentTests.cs
tests/Td.Core.Tests/Content/WaveGenerationRulesTests.cs
```

Критерии завершения:

```text
- ровно 10 монстров;
- ровно 4 башни;
- ровно 3 уровня прокачки у каждой башни;
- уникальные Id;
- корректные диапазоны параметров;
- dotnet build проходит;
- dotnet test проходит;
- числа являются плейсхолдерами до этапа балансировки.
```

Важная техническая заметка:

```text
Перед завершением N4 необходимо убедиться, что пространства имен в домене и контенте согласованы.
Например:
- Td.Core.Domain.Monsters
- Td.Core.Domain.Towers
- Td.Core.Content
```

---

## 8. Следующие шаги после `N4`

После успешного завершения `N4` планируется переход к:

```text
N5 Mechanics/status contracts
```

На `N5` будут уточнены и формализованы:

- урон;
- броня;
- бронебойность;
- замедление;
- сопротивление замедлению;
- полет;
- площадной урон;
- статусы.

---

## 9. Топологический порядок

Базовый порядок выполнения:

```text
N0
→ N1
→ N2
→ N3, N4
→ N5
→ N6
→ N7
→ N8
→ N9
→ N10
→ N11, N12
→ N13, N14
→ N15
→ N16
→ N17
→ N18
```

Фактический текущий порядок:

```text
N0 DONE
N1 DONE
N2 DONE
N3 DONE
N4 ACTIVE
N5 PENDING
N6 PENDING
N7 PENDING
N8 PENDING
N9 PENDING
N10 PENDING
N11 PENDING
N12 PENDING
N13 PENDING
N14 PENDING
N15 PENDING
N16 PENDING
N17 PENDING
N18 PENDING
```

---

## 10. Список ребер графа

```text
N0 -> N1
N1 -> N2
N0 -> N3
N2 -> N3
N0 -> N4
N2 -> N4
N2 -> N5
N2 -> N6
N3 -> N6
N4 -> N6
N5 -> N6
N4 -> N7
N5 -> N7
N6 -> N7
N1 -> N8
N6 -> N8
N6 -> N9
N8 -> N9
N3 -> N10
N6 -> N10
N7 -> N10
N8 -> N10
N9 -> N10
N1 -> N11
N10 -> N11
N1 -> N12
N10 -> N12
N3 -> N13
N10 -> N13
N4 -> N14
N7 -> N14
N10 -> N14
N4 -> N15
N7 -> N15
N10 -> N15
N6 -> N16
N7 -> N16
N9 -> N16
N10 -> N16
N11 -> N16
N12 -> N16
N13 -> N16
N14 -> N16
N15 -> N16
N1 -> N17
N16 -> N17
N16 -> N18
N17 -> N18
```

---

## 11. Критический путь

Ориентировочный критический путь:

```text
N0
→ N1
→ N2
→ N3/N4
→ N5
→ N6
→ N8
→ N9
→ N10
→ N15
→ N16
→ N18
```

Волновой генератор `N7`, звук `N11`, настройки `N12` и редакторы `N13/N14` могут частично развиваться параллельно, но финальная приемка зависит от интеграции и тестирования.

---

## 12. Этапы проекта

### Этап A — Core Playable

Цель: играбельный каркас без редакторов.

Сюда входят:

```text
N0
N1
N2
N3
N4
N5
N6
N7
N8
N9
N10
```

Текущее состояние:

```text
этап в работе
выполнено: N0, N1, N2, N3
активно: N4
```

---

### Этап B — Full Game

Цель: довести игру до принятых требований.

Сюда входят:

```text
N11
N12
N15
```

---

### Этап C — Editors and Hardening

Цель: редакторы, стабилизация, автосборка, приемка.

Сюда входят:

```text
N13
N14
N16
N17
N18
```

---

## 13. Известные риски

| Риск | Уровень | Комментарий |
|---|---:|---|
| Пространства имен в `Content` и `Domain` могут расходиться | средний | нужно держать единый стиль `Td.Core.Domain.*` и `Td.Core.Content` |
| Плейсхолдерный баланс | средний | тюнинг будет на `N15` |
| Генерация случайных волн может ломать рост сложности | высокий | нужно тестировать на `N7` |
| WPF-производительность | средний | проверить при отрисовке монстров, башен и эффектов |
| Редакторы могут сильно расширить скоуп | высокий | они вынесены в этап C |
| `.NET 10` окружение | низкий/средний | уже подтверждено на `N1`, но нужно держать под контролем |

---

## 14. Рекомендуемый порядок работы прямо сейчас

Сейчас нужно завершить:

```text
N4 Content catalog
```

После завершения:

```bash
dotnet build
dotnet test
```

Ожидаемый рапорт:

```text
DONE N4: content catalog created, 10 monsters, 4 towers, 3 upgrade levels, tests ok
```

После этого детализируется:

```text
N5 Mechanics/status contracts
```