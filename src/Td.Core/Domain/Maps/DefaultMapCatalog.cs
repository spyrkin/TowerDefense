/// <summary>
/// Каталог тестовых карт по умолчанию.
/// Содержит 3 временные карты для проверки модели нескольких карт,
/// геометрии пути и валидации размещения башен.
///
/// ВАЖНО: геометрия этих карт является временной (test-only placeholder)
/// и НЕ считается финальным геймдизайном.
/// Финальная геометрия карт должна быть утверждена отдельно.
/// </summary>
public sealed class DefaultMapCatalog : IMapCatalog
{
    private readonly IReadOnlyList<MapDefinition> _maps;

    public DefaultMapCatalog()
    {
        _maps = CreateDefaultMaps();
    }

    public IReadOnlyList<MapDefinition> GetMaps() => _maps;

    public MapDefinition? GetMap(string id)
    {
        foreach (var map in _maps)
        {
            if (map.Id == id)
            {
                return map;
            }
        }
        return null;
    }

    /// <summary>
    /// Создаёт набор из 3 временных тестовых карт.
    /// Эти карты предназначены для проверки:
    /// - модели нескольких карт;
    /// - геометрии пути;
    /// - валидации размещения башен;
    /// - каталога карт.
    ///
    /// Карты НЕ являются финальным геймдизайном.
    /// </summary>
    public static IReadOnlyList<MapDefinition> CreateDefaultMaps()
    {
        return new List<MapDefinition>
        {
            CreateMapStraight(),
            CreateMapCorner(),
            CreateMapSerpentine()
        };
    }

    // ------------------------------------------------------------------
    // Карта 1: Прямой путь через центр экрана слева направо
    // ------------------------------------------------------------------
    /// <summary>
    /// Тестовая карта с прямым горизонтальным путём через центр экрана.
    /// Путь: (0, 360) → (1280, 360)
    /// </summary>
    private static MapDefinition CreateMapStraight()
    {
        return new MapDefinition
        {
            Id = "map_test_straight",
            Name = "Test Map: Straight",
            Width = 1280,
            Height = 720,
            PathBufferRadius = 48,
            Path = new List<Point2D>
            {
                new Point2D(0, 360),
                new Point2D(1280, 360)
            }
        };
    }

    // ------------------------------------------------------------------
    // Карта 2: Г-образный путь с поворотом
    // ------------------------------------------------------------------
    /// <summary>
    /// Тестовая карта с Г-образным путём.
    /// Путь: (0, 120) → (640, 120) → (640, 600) → (1280, 600)
    /// </summary>
    private static MapDefinition CreateMapCorner()
    {
        return new MapDefinition
        {
            Id = "map_test_corner",
            Name = "Test Map: Corner",
            Width = 1280,
            Height = 720,
            PathBufferRadius = 48,
            Path = new List<Point2D>
            {
                new Point2D(0, 120),
                new Point2D(640, 120),
                new Point2D(640, 600),
                new Point2D(1280, 600)
            }
        };
    }

    // ------------------------------------------------------------------
    // Карта 3: Серпантинный путь с несколькими поворотами
    // ------------------------------------------------------------------
    /// <summary>
    /// Тестовая карта с серпантинным (змеевидным) путём.
    /// Путь: (0, 120) → (960, 120) → (960, 360) → (320, 360) → (320, 600) → (1280, 600)
    /// </summary>
    private static MapDefinition CreateMapSerpentine()
    {
        return new MapDefinition
        {
            Id = "map_test_serpentine",
            Name = "Test Map: Serpentine",
            Width = 1280,
            Height = 720,
            PathBufferRadius = 48,
            Path = new List<Point2D>
            {
                new Point2D(0, 120),
                new Point2D(960, 120),
                new Point2D(960, 360),
                new Point2D(320, 360),
                new Point2D(320, 600),
                new Point2D(1280, 600)
            }
        };
    }
}