public interface IDamageCalculator
{
    int CalculateDamage(
        TowerLevelDefinition towerLevel,
        MonsterType monsterType);
}