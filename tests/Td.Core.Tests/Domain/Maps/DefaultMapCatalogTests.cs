using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Тесты каталога тестовых карт по умолчанию (<see cref="DefaultMapCatalog"/>).
/// Проверяют:
/// - количество карт;
/// - уникальность и непустоту идентификаторов;
/// - непустоту названий;
/// - корректность размеров;
/// - корректность путей;
/// - корректность буферной зоны пути;
/// - поведение GetMap для известных и неизвестных Id.
/// </summary>
public sealed class DefaultMapCatalogTests
{
    private readonly DefaultMapCatalog _catalog = new();

    // ------------------------------------------------------------------
    // Количество карт
    // ------------------------------------------------------------------

    [Fact]
    public void GetMaps_ReturnsExactlyThreeMaps()
    {
        IReadOnlyList<MapDefinition> maps = _catalog.GetMaps();

        Assert.Equal(3, maps.Count);
    }

    [Fact]
    public void CreateDefaultMaps_ReturnsExactlyThreeMaps()
    {
        IReadOnlyList<MapDefinition> maps = DefaultMapCatalog.CreateDefaultMaps();

        Assert.Equal(3, maps.Count);
    }

    // ------------------------------------------------------------------
    // Уникальность и непустота Id
    // ------------------------------------------------------------------

    [Fact]
    public void GetMaps_AllIdsAreUnique()
    {
        IReadOnlyList<MapDefinition> maps = _catalog.GetMaps();
        List<string> ids = maps.Select(m => m.Id).ToList();

        Assert.Equal(ids.Count, ids.Distinct().Count());
    }

    [Fact]
    public void GetMaps_AllIdsAreNonEmpty()
    {
        foreach (MapDefinition map in _catalog.GetMaps())
        {
            Assert.False(string.IsNullOrWhiteSpace(map.Id));
        }
    }

    [Fact]
    public void GetMaps_ContainsExpectedMapIds()
    {
        List<string> ids = _catalog.GetMaps().Select(m => m.Id).ToList();

        Assert.Contains("map_test_straight", ids);
        Assert.Contains("map_test_corner", ids);
        Assert.Contains("map_test_serpentine", ids);
    }

    // ------------------------------------------------------------------
    // Непустота Name
    // ------------------------------------------------------------------

    [Fact]
    public void GetMaps_AllNamesAreNonEmpty()
    {
        foreach (MapDefinition map in _catalog.GetMaps())
        {
            Assert.False(string.IsNullOrWhiteSpace(map.Name));
        }
    }

    // ------------------------------------------------------------------
    // Размеры карты
    // ------------------------------------------------------------------

    [Fact]
    public void GetMaps_AllDimensionsArePositive()
    {
        foreach (MapDefinition map in _catalog.GetMaps())
        {
            Assert.True(map.Width > 0, $"Map '{map.Id}' must have positive Width.");
            Assert.True(map.Height > 0, $"Map '{map.Id}' must have positive Height.");
        }
    }

    // ------------------------------------------------------------------
    // Путь
    // ------------------------------------------------------------------

    [Fact]
    public void GetMaps_AllPathsHaveAtLeastTwoPoints()
    {
        foreach (MapDefinition map in _catalog.GetMaps())
        {
            Assert.NotNull(map.Path);
            Assert.True(
                map.Path.Count >= 2,
                $"Map '{map.Id}' must have a path with at least 2 points.");
        }
    }

    [Fact]
    public void GetMaps_AllPathPointsAreWithinMapBounds()
    {
        foreach (MapDefinition map in _catalog.GetMaps())
        {
            foreach (Point2D point in map.Path)
            {
                Assert.InRange(point.X, 0, map.Width);
                Assert.InRange(point.Y, 0, map.Height);
            }
        }
    }

    // ------------------------------------------------------------------
    // Буферная зона пути
    // ------------------------------------------------------------------

    [Fact]
    public void GetMaps_AllPathBufferRadiiAreNonNegative()
    {
        foreach (MapDefinition map in _catalog.GetMaps())
        {
            Assert.True(
                map.PathBufferRadius >= 0,
                $"Map '{map.Id}' must have non-negative PathBufferRadius.");
        }
    }

    // ------------------------------------------------------------------
    // GetMap
    // ------------------------------------------------------------------

    [Fact]
    public void GetMap_ReturnsMapForKnownId()
    {
        foreach (MapDefinition map in _catalog.GetMaps())
        {
            MapDefinition? found = _catalog.GetMap(map.Id);

            Assert.NotNull(found);
            Assert.Equal(map.Id, found.Id);
        }
    }

    [Fact]
    public void GetMap_ReturnsNullForUnknownId()
    {
        MapDefinition? found = _catalog.GetMap("unknown_map_id");

        Assert.Null(found);
    }

    // ------------------------------------------------------------------
    // Консистентность
    // ------------------------------------------------------------------

    [Fact]
    public void GetMaps_ReturnsSameInstanceOnMultipleCalls()
    {
        IReadOnlyList<MapDefinition> first = _catalog.GetMaps();
        IReadOnlyList<MapDefinition> second = _catalog.GetMaps();

        Assert.Same(first, second);
    }

    [Fact]
    public void GetMaps_MatchesCreateDefaultMapsById()
    {
        IReadOnlyList<MapDefinition> catalogMaps = _catalog.GetMaps();
        IReadOnlyList<MapDefinition> factoryMaps = DefaultMapCatalog.CreateDefaultMaps();

        Assert.Equal(factoryMaps.Count, catalogMaps.Count);
        for (int i = 0; i < catalogMaps.Count; i++)
        {
            Assert.Equal(factoryMaps[i].Id, catalogMaps[i].Id);
        }
    }
}