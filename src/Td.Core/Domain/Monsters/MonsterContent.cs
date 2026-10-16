using System.Collections.Generic;

public static class MonsterContent
{
    public static IReadOnlyList<MonsterType> All { get; } = new List<MonsterType>
    {
        new MonsterType
        {
            Id = "monster_t1_grunt",
            Name = "Grunt",
            Class = MonsterClass.Normal,
            MaxHp = 70,
            Speed = 60.0,
            Armor = 0,
            GoldReward = 8,
            SlowResistance = 0.0,
            Tier = 1
        },

        new MonsterType
        {
            Id = "monster_t1_scout",
            Name = "Scout",
            Class = MonsterClass.Fast,
            MaxHp = 45,
            Speed = 95.0,
            Armor = 0,
            GoldReward = 9,
            SlowResistance = 0.0,
            Tier = 1
        },

        new MonsterType
        {
            Id = "monster_t2_runner",
            Name = "Runner",
            Class = MonsterClass.Fast,
            MaxHp = 80,
            Speed = 105.0,
            Armor = 0,
            GoldReward = 12,
            SlowResistance = 0.0,
            Tier = 2
        },

        new MonsterType
        {
            Id = "monster_t2_brute",
            Name = "Brute",
            Class = MonsterClass.Tank,
            MaxHp = 240,
            Speed = 42.0,
            Armor = 3,
            GoldReward = 16,
            SlowResistance = 0.05,
            Tier = 2
        },

        new MonsterType
        {
            Id = "monster_t3_armored",
            Name = "Armored",
            Class = MonsterClass.Normal,
            MaxHp = 200,
            Speed = 55.0,
            Armor = 5,
            GoldReward = 18,
            SlowResistance = 0.05,
            Tier = 3
        },

        new MonsterType
        {
            Id = "monster_t3_flyer",
            Name = "Flyer",
            Class = MonsterClass.Flying,
            MaxHp = 120,
            Speed = 82.0,
            Armor = 1,
            GoldReward = 15,
            SlowResistance = 0.0,
            Tier = 3
        },

        new MonsterType
        {
            Id = "monster_t4_guard",
            Name = "Guard",
            Class = MonsterClass.Tank,
            MaxHp = 460,
            Speed = 36.0,
            Armor = 8,
            GoldReward = 25,
            SlowResistance = 0.10,
            Tier = 4
        },

        new MonsterType
        {
            Id = "monster_t4_skyflyer",
            Name = "Skyflyer",
            Class = MonsterClass.Flying,
            MaxHp = 170,
            Speed = 112.0,
            Armor = 2,
            GoldReward = 22,
            SlowResistance = 0.10,
            Tier = 4
        },

        new MonsterType
        {
            Id = "monster_t5_juggernaut",
            Name = "Juggernaut",
            Class = MonsterClass.Tank,
            MaxHp = 850,
            Speed = 30.0,
            Armor = 12,
            GoldReward = 40,
            SlowResistance = 0.20,
            Tier = 5
        },

        new MonsterType
        {
            Id = "monster_t5_alpha",
            Name = "Alpha",
            Class = MonsterClass.Normal,
            MaxHp = 380,
            Speed = 66.0,
            Armor = 6,
            GoldReward = 32,
            SlowResistance = 0.15,
            Tier = 5
        }
    };
}