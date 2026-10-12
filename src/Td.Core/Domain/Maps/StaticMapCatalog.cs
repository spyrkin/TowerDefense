public sealed class StaticMapCatalog : IMapCatalog
{
    private readonly IReadOnlyList<MapDefinition> _maps;

    public StaticMapCatalog(IReadOnlyList<MapDefinition> maps)
    {
        _maps = maps;
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
}