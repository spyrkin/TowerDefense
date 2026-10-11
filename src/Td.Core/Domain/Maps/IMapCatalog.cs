public interface IMapCatalog
{
    IReadOnlyList<MapDefinition> GetMaps();
    MapDefinition? GetMap(string id);
}