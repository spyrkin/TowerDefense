public interface IPlacementValidator
{
    bool CanPlaceTower(
        MapDefinition map,
        Point2D position,
        TowerType towerType,
        IReadOnlyCollection<TowerState> existingTowers);
}