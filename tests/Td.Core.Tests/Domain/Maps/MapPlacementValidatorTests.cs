using System;
using System.Collections.Generic;

/// <summary>
/// Тесты валидатора размещения башен (<see cref="MapPlacementValidator"/>).
///
/// Проверяют четыре группы правил, зафиксированных в интерфейсе
/// <see cref="IPlacementValidator"/> (шаг R04):
///
/// 1. Input Check        — корректность входных данных;
/// 2. Bounds Check       — позиция вместе с FootprintRadius внутри карты;
/// 3. Path Buffer Check  — позиция вне буферной зоны пути;
/// 4. Tower Collision    — позиция не пересекается с существующими башнями.
///
/// Тестовая геометрия:
///   Карта 1000x1000, горизонтальный путь по линии y = 500.
///   PathBufferRadius = 50, FootprintRadius башни = 25.
///
///   Для валидного размещения позиция должна одновременно удовлетворять:
///     Bounds:     x in [25, 975], y in [25, 975]
///     PathBuffer: |y - 500| >= 50 + 25 = 75, т.е. y <= 425 или y >= 575
/// </summary>
public sealed class MapPlacementValidatorTests
{
    private const double MapSize = 1000.0;
    private const double PathY = 500.0;
    private const double PathBufferRadius = 50.0;
    private const double TowerRadius = 25.0;

    private readonly MapPlacementValidator _validator = new(new PathGeometry());

    private static readonly IReadOnlyCollection<TowerState> NoTowers = new List<TowerState>();

    // ------------------------------------------------------------------
    // 1. Input Check
    // ------------------------------------------------------------------

    [Fact]
    public void CanPlaceTower_NullMap_ReturnsFalse()
    {
        MapDefinition map = null!;

        bool result = _validator.CanPlaceTower(
            map,
            new Point2D(100, 100),
            CreateTowerType(),
            NoTowers);

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_NullTowerType_ReturnsFalse()
    {
        TowerType towerType = null!;

        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 100),
            towerType,
            NoTowers);

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_NullExistingTowers_ReturnsFalse()
    {
        IReadOnlyCollection<TowerState> towers = null!;

        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 100),
            CreateTowerType(),
            towers);

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_PathWithSinglePoint_ReturnsFalse()
    {
        MapDefinition map = new()
        {
            Id = "map_test",
            Name = "Test Map",
            Width = MapSize,
            Height = MapSize,
            PathBufferRadius = PathBufferRadius,
            Path = new List<Point2D>
            {
                new(0, PathY)
            }
        };

        bool result = _validator.CanPlaceTower(
            map,
            new Point2D(100, 100),
            CreateTowerType(),
            NoTowers);

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_NullPath_ReturnsFalse()
    {
        MapDefinition map = new()
        {
            Id = "map_test",
            Name = "Test Map",
            Width = MapSize,
            Height = MapSize,
            PathBufferRadius = PathBufferRadius,
            Path = null!
        };

        bool result = _validator.CanPlaceTower(
            map,
            new Point2D(100, 100),
            CreateTowerType(),
            NoTowers);

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_ZeroFootprintRadius_ReturnsFalse()
    {
        TowerType towerType = CreateTowerType(footprintRadius: 0.0);

        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 100),
            towerType,
            NoTowers);

        Assert.False(result);
    }

    // ------------------------------------------------------------------
    // 2. Bounds Check
    // ------------------------------------------------------------------

    [Fact]
    public void CanPlaceTower_ValidPosition_ReturnsTrue()
    {
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 100),
            CreateTowerType(),
            NoTowers);

        Assert.True(result);
    }

    [Fact]
    public void CanPlaceTower_TooCloseToLeftEdge_ReturnsFalse()
    {
        // x = 10 < towerRadius (25)
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(10, 100),
            CreateTowerType(),
            NoTowers);

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_TooCloseToRightEdge_ReturnsFalse()
    {
        // x = 990 > Width - towerRadius (975)
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(990, 100),
            CreateTowerType(),
            NoTowers);

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_TooCloseToTopEdge_ReturnsFalse()
    {
        // y = 10 < towerRadius (25)
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 10),
            CreateTowerType(),
            NoTowers);

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_TooCloseToBottomEdge_ReturnsFalse()
    {
        // y = 990 > Height - towerRadius (975)
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 990),
            CreateTowerType(),
            NoTowers);

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_PositionExactlyAtMapBoundary_ReturnsTrue()
    {
        // x = 25 == towerRadius (граница включается, т.к. используется >=)
        // y = 100 далеко от пути
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(TowerRadius, 100),
            CreateTowerType(),
            NoTowers);

        Assert.True(result);
    }

    // ------------------------------------------------------------------
    // 3. Path Buffer Check
    // ------------------------------------------------------------------

    [Fact]
    public void CanPlaceTower_PositionOnPath_ReturnsFalse()
    {
        // (500, 500) лежит прямо на пути; distance = 0 < 75
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(500, 500),
            CreateTowerType(),
            NoTowers);

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_PositionWithinPathBuffer_ReturnsFalse()
    {
        // (500, 550): distance = 50 < PathBufferRadius + TowerRadius = 75
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(500, 550),
            CreateTowerType(),
            NoTowers);

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_PositionExactlyAtPathBufferBoundary_ReturnsTrue()
    {
        // (500, 425): distance = |425 - 500| = 75 == PathBufferRadius + TowerRadius
        double y = PathY - (PathBufferRadius + TowerRadius);

        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(500, y),
            CreateTowerType(),
            NoTowers);

        Assert.True(result);
    }

    // ------------------------------------------------------------------
    // 4. Tower Collision Check
    // ------------------------------------------------------------------

    [Fact]
    public void CanPlaceTower_ExistingTowerFarAway_ReturnsTrue()
    {
        TowerType type = CreateTowerType();
        TowerState existing = CreateTower(type, new Point2D(900, 900));

        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 100),
            type,
            new List<TowerState> { existing });

        Assert.True(result);
    }

    [Fact]
    public void CanPlaceTower_ExistingTowerOverlapping_ReturnsFalse()
    {
        TowerType type = CreateTowerType();
        TowerState existing = CreateTower(type, new Point2D(120, 100));

        // distance = 20 < TowerRadius + TowerRadius = 50
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 100),
            type,
            new List<TowerState> { existing });

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_ExistingTowerExactlyAtCollisionBoundary_ReturnsTrue()
    {
        TowerType type = CreateTowerType();
        TowerState existing = CreateTower(type, new Point2D(150, 100));

        // distance = 50 == TowerRadius + TowerRadius (граница включается)
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 100),
            type,
            new List<TowerState> { existing });

        Assert.True(result);
    }

    [Fact]
    public void CanPlaceTower_DifferentFootprintRadii_UsesSumOfRadii()
    {
        TowerType newTowerType = CreateTowerType(footprintRadius: 30.0);
        TowerType existingType = CreateTowerType(footprintRadius: 20.0);
        TowerState existing = CreateTower(existingType, new Point2D(140, 100));

        // distance = 40 < 30 + 20 = 50
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 100),
            newTowerType,
            new List<TowerState> { existing });

        Assert.False(result);
    }

    [Fact]
    public void CanPlaceTower_NullTowerInCollection_IsIgnored()
    {
        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 100),
            CreateTowerType(),
            new List<TowerState> { null! });

        Assert.True(result);
    }

    [Fact]
    public void CanPlaceTower_TowerWithNullTypeInCollection_IsIgnored()
    {
        TowerState broken = new()
        {
            Id = Guid.NewGuid(),
            Type = null!,
            Position = new Point2D(100, 100)
        };

        bool result = _validator.CanPlaceTower(
            CreateMap(),
            new Point2D(100, 100),
            CreateTowerType(),
            new List<TowerState> { broken });

        Assert.True(result);
    }

    // ------------------------------------------------------------------
    // Helpers
    // ------------------------------------------------------------------

    private static MapDefinition CreateMap()
    {
        return new MapDefinition
        {
            Id = "map_test",
            Name = "Test Map",
            Width = MapSize,
            Height = MapSize,
            PathBufferRadius = PathBufferRadius,
            Path = new List<Point2D>
            {
                new(0, PathY),
                new(MapSize, PathY)
            }
        };
    }

    private static TowerType CreateTowerType(double footprintRadius = TowerRadius)
    {
        return new TowerType
        {
            Id = "tower_test",
            Name = "Test Tower",
            Kind = TowerKind.Normal,
            BaseCost = 100,
            FootprintRadius = footprintRadius,
            Levels = new List<TowerLevelDefinition>
            {
                new TowerLevelDefinition
                {
                    Level = 1,
                    UpgradeCost = 0,
                    Damage = 10,
                    Range = 100.0,
                    CooldownSeconds = 1.0,
                    SlowPercent = 0.0,
                    SlowDurationSeconds = 0.0,
                    SplashRadius = 0.0,
                    ArmorIgnorePercent = 0.0
                }
            }
        };
    }

    private static TowerState CreateTower(TowerType type, Point2D position)
    {
        return new TowerState
        {
            Id = Guid.NewGuid(),
            Type = type,
            Position = position
        };
    }
}