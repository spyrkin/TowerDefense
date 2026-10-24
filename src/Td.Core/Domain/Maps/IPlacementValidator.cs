/// <summary>
/// Валидатор размещения башен на карте.
/// Инкапсулирует правила коллизий с путем, границами карты и другими башнями.
/// </summary>
public interface IPlacementValidator
{
    /// <summary>
    /// Проверяет, можно ли разместить башню в указанной позиции.
    /// </summary>
    /// <param name="map">
    /// Определение карты.
    /// Должно содержать корректный путь минимум из двух точек,
    /// а также размеры карты и <c>PathBufferRadius</c>.
    /// </param>
    /// <param name="position">
    /// Координаты центра предполагаемой башни.
    /// </param>
    /// <param name="towerType">
    /// Тип башни.
    /// Используется для получения радиуса занимаемой области через <c>FootprintRadius</c>.
    /// </param>
    /// <param name="existingTowers">
    /// Коллекция уже размещенных на карте башен.
    /// </param>
    /// <returns>
    /// <c>true</c>, если размещение валидно; иначе <c>false</c>.
    /// <para>
    /// Строгие правила валидации, соответствующие реализации <c>MapPlacementValidator</c>:
    /// </para>
    /// <para>
    /// 1. Input Check:
    /// <list type="bullet">
    /// <item><description><c>map</c> не равен <c>null</c>.</description></item>
    /// <item><description><c>towerType</c> не равен <c>null</c>.</description></item>
    /// <item><description><c>existingTowers</c> не равен <c>null</c>.</description></item>
    /// <item><description><c>map.Path</c> не равен <c>null</c> и содержит минимум 2 точки.</description></item>
    /// <item><description><c>towerType.FootprintRadius</c> больше 0.</description></item>
    /// </list>
    /// При нарушении любого из этих условий метод возвращает <c>false</c>.
    /// </para>
    /// <para>
    /// 2. Bounds Check:
    /// позиция башни вместе с её <c>FootprintRadius</c> должна полностью находиться внутри карты.
    /// <code>
    /// position.X &gt;= towerRadius
    /// position.X &lt;= map.Width - towerRadius
    /// position.Y &gt;= towerRadius
    /// position.Y &lt;= map.Height - towerRadius
    /// </code>
    /// где <c>towerRadius = towerType.FootprintRadius</c>.
    /// </para>
    /// <para>
    /// 3. Path Buffer Check:
    /// евклидово расстояние от <c>position</c> до ближайшей точки пути
    /// должно быть не меньше суммы буферной зоны пути и радиуса башни.
    /// <code>
    /// distanceToPath &gt;= map.PathBufferRadius + towerRadius
    /// </code>
    /// </para>
    /// <para>
    /// 4. Tower Collision Check:
    /// евклидово расстояние от <c>position</c> до центра любой уже размещенной башни
    /// должно быть не меньше суммы радиуса новой башни и радиуса существующей башни.
    /// <code>
    /// distance &gt;= towerRadius + existingTowerRadius
    /// </code>
    /// где <c>existingTowerRadius = existingTower.Type.FootprintRadius</c>.
    /// </para>
    /// <para>
    /// Примечание:
    /// существующие башни, у которых сама башня или её тип равны <c>null</c>,
    /// пропускаются при проверке коллизий.
    /// </para>
    /// </returns>
    bool CanPlaceTower(
        MapDefinition map,
        Point2D position,
        TowerType towerType,
        IReadOnlyCollection<TowerState> existingTowers);
}