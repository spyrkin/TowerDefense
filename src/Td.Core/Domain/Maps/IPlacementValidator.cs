

/// <summary>
/// Валидатор размещения башен на карте. 
/// Инкапсулирует правила коллизий с путем, границами карты и другими башнями.
/// </summary>
public interface IPlacementValidator
{
    /// <summary>
    /// Проверяет, можно ли разместить башню в указанной позиции.
    /// </summary>
    /// <param name="map">Определение карты (содержит путь, буферную зону и границы).</param>
    /// <param name="position">Координаты центра предполагаемой башни.</param>
    /// <param name="towerType">Тип башни (резерв для будущих специфических правил, например, для летающих башен).</param>
    /// <param name="existingTowers">Коллекция уже размещенных на карте башен.</param>
    /// <returns>
    /// true, если размещение валидно.
    /// <para>
    /// Строгие правила валидации (Spec-first):
    /// 1. Bounds Check: position.X in [0, map.Width], position.Y in [0, map.Height].
    /// 2. Path Buffer Check: Евклидово расстояние от position до ближайшей точки на любом сегменте map.Path 
    ///    должно быть строго >= map.PathBufferRadius.
    /// 3. Tower Collision Check: Евклидово расстояние от position до центра любой башни из existingTowers 
    ///    должно быть строго >= MechanicsLimits.MinTowerDistance.
    /// </para>
    /// </returns>
    bool CanPlaceTower(
        MapDefinition map,
        Point2D position,
        TowerType towerType,
        IReadOnlyCollection<TowerState> existingTowers);
}