public sealed class MonsterTypeTests
{
    [Theory]
    [InlineData(MonsterClass.Normal, false)]
    [InlineData(MonsterClass.Fast, false)]
    [InlineData(MonsterClass.Tank, false)]
    [InlineData(MonsterClass.Flying, true)]
    public void IsFlying_DependsOnMonsterClass(MonsterClass monsterClass, bool expected)
    {
        var monsterType = new MonsterType
        {
            Id = "monster_test",
            Name = "Test Monster",
            Class = monsterClass,
            MaxHp = 100,
            Speed = 50.0,
            Armor = 0,
            GoldReward = 10,
            SlowResistance = 0.0,
            Tier = 1
        };

        Assert.Equal(expected, monsterType.IsFlying);
    }
}