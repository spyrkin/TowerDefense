using System;
using System.Collections.Generic;




public sealed class MapPlacementValidator : IPlacementValidator
{
    private readonly IPathGeometry _pathGeometry;

    public MapPlacementValidator(IPathGeometry pathGeometry)
    {
        _pathGeometry = pathGeometry ?? throw new ArgumentNullException(nameof(pathGeometry));
    }

    public bool CanPlaceTower(
        MapDefinition map,
        Point2D position,
        TowerType towerType,
        IReadOnlyCollection<TowerState> existingTowers)
    {
        if (map is null)
        {
            return false;
        }

        if (towerType is null)
        {
            return false;
        }

        if (existingTowers is null)
        {
            return false;
        }

        if (map.Path is null || map.Path.Count < 2)
        {
            return false;
        }

        double towerRadius = towerType.FootprintRadius;

        if (towerRadius <= 0)
        {
            return false;
        }

        if (!IsInsideMap(map, position, towerRadius))
        {
            return false;
        }

        if (!IsAwayFromPath(map, position, towerRadius))
        {
            return false;
        }

        if (!IsNotOverlappingExistingTowers(position, towerRadius, existingTowers))
        {
            return false;
        }

        return true;
    }

    private static bool IsInsideMap(
        MapDefinition map,
        Point2D position,
        double towerRadius)
    {
        return
            position.X >= towerRadius &&
            position.X <= map.Width - towerRadius &&
            position.Y >= towerRadius &&
            position.Y <= map.Height - towerRadius;
    }

    private bool IsAwayFromPath(
        MapDefinition map,
        Point2D position,
        double towerRadius)
    {
        double distanceToPath = _pathGeometry.GetDistanceToPath(map.Path, position);

        double requiredDistance = map.PathBufferRadius + towerRadius;

        return distanceToPath >= requiredDistance;
    }

    private static bool IsNotOverlappingExistingTowers(
        Point2D position,
        double towerRadius,
        IReadOnlyCollection<TowerState> existingTowers)
    {
        foreach (TowerState tower in existingTowers)
        {
            if (tower is null)
            {
                continue;
            }

            if (tower.Type is null)
            {
                continue;
            }

            double otherRadius = tower.Type.FootprintRadius;

            double requiredDistance = towerRadius + otherRadius;

            double distanceSquared = position.DistanceSquaredTo(tower.Position);

            if (distanceSquared < requiredDistance * requiredDistance)
            {
                return false;
            }
        }

        return true;
    }
}