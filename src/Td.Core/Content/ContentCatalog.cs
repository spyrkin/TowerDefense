using System.Collections.Generic;



public static class ContentCatalog
{
    public static IReadOnlyList<MonsterType> Monsters => MonsterContent.All;

    public static IReadOnlyList<TowerType> Towers => TowerContent.All;
}