using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Базовый симуляционный движок:
/// спавн монстров, движение, замедление, атака башен, урон, смерть, утечка.
/// </summary>
public sealed class GameSimulationEngine : ISimulationEngine
{
    private const double ComparisonEpsilon = 0.0001;

    private readonly IPathGeometry _pathGeometry;
    private readonly IDamageCalculator _damageCalculator;
    private readonly IReadOnlyList<WaveDefinition> _waves;
    private readonly Dictionary<string, MonsterType> _monsterCatalogById;
    private readonly Queue<SpawnRequest> _spawnQueue = new();

    private double _spawnTimer;

    public GameSimulationEngine(
        IPathGeometry pathGeometry,
        IDamageCalculator damageCalculator,
        IReadOnlyList<MonsterType> monsterCatalog,
        IReadOnlyList<WaveDefinition> waves)
    {
        _pathGeometry = pathGeometry ?? throw new ArgumentNullException(nameof(pathGeometry));
        _damageCalculator = damageCalculator ?? throw new ArgumentNullException(nameof(damageCalculator));
        _waves = waves ?? throw new ArgumentNullException(nameof(waves));

        if (monsterCatalog is null)
        {
            throw new ArgumentNullException(nameof(monsterCatalog));
        }

        _monsterCatalogById = monsterCatalog.ToDictionary(monster => monster.Id, monster => monster);
    }

    public void StartNextWave(GameState state)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (state.Phase != GamePhase.Playing)
        {
            return;
        }

        if (_spawnQueue.Count > 0 || state.Monsters.Any(monster => monster.IsAlive))
        {
            return;
        }

        int maxWaves = Math.Min(state.TotalWaves, _waves.Count);

        if (state.CurrentWave >= maxWaves)
        {
            return;
        }

        state.StartNextWave();

        WaveDefinition wave = _waves[state.CurrentWave - 1];
        EnqueueWave(wave);
    }

    public void Update(GameState state, double deltaSeconds)
    {
        ArgumentNullException.ThrowIfNull(state);

        if (state.Phase != GamePhase.Playing)
        {
            return;
        }

        if (deltaSeconds <= 0)
        {
            return;
        }

        ProcessSpawns(state, deltaSeconds);

        UpdateMonsters(state, deltaSeconds);

        if (state.Phase != GamePhase.Playing)
        {
            return;
        }

        UpdateTowers(state, deltaSeconds);
    }

    private void EnqueueWave(WaveDefinition wave)
    {
        foreach (WaveEntry entry in wave.Entries)
        {
            int count = Math.Max(0, entry.Count);

            for (int i = 0; i < count; i++)
            {
                _spawnQueue.Enqueue(new SpawnRequest(entry.MonsterTypeId));
            }
        }

        _spawnTimer = 0.0;
    }

    private void ProcessSpawns(GameState state, double deltaSeconds)
    {
        if (_spawnQueue.Count == 0)
        {
            _spawnTimer = 0.0;
            return;
        }

        _spawnTimer -= deltaSeconds;

        double interval = Math.Max(0.01, WaveGenerationRules.SpawnIntervalSeconds);

        while (_spawnQueue.Count > 0 && _spawnTimer <= 0.0)
        {
            SpawnRequest request = _spawnQueue.Dequeue();

            if (_monsterCatalogById.TryGetValue(request.MonsterTypeId, out MonsterType? monsterType))
            {
                state.AddMonster(CreateMonster(state, monsterType));
            }

            _spawnTimer += interval;
        }
    }

    private static MonsterState CreateMonster(GameState state, MonsterType type)
    {
        Point2D startPosition = state.Map.Path.Count > 0
            ? state.Map.Path[0]
            : Point2D.Zero;

        return new MonsterState
        {
            Id = Guid.NewGuid(),
            Type = type,
            Position = startPosition,
            CurrentHp = type.MaxHp,
            PathProgress = 0.0
        };
    }

    private void UpdateMonsters(GameState state, double deltaSeconds)
    {
        IReadOnlyList<Point2D> path = state.Map.Path;
        double pathLength = _pathGeometry.GetLength(path);

        List<MonsterState> leaked = new();

        foreach (MonsterState monster in state.Monsters)
        {
            if (!monster.IsAlive)
            {
                continue;
            }

            monster.TickSlow(deltaSeconds);

            double slowPercent = monster.Slow?.Percent ?? 0.0;
            double speedFactor = Math.Clamp(1.0 - slowPercent, 0.0, 1.0);

            double moveDistance = monster.Type.Speed * speedFactor * deltaSeconds;

            if (pathLength <= 0.0)
            {
                monster.PathProgress = 1.0;
            }
            else
            {
                monster.PathProgress += moveDistance / pathLength;
            }

            if (monster.PathProgress >= 1.0)
            {
                monster.Position = path.Count > 0 ? path[^1] : Point2D.Zero;
                leaked.Add(monster);
            }
            else
            {
                monster.Position = _pathGeometry.GetPointAtDistance(
                    path,
                    monster.PathProgress * pathLength);
            }
        }

        foreach (MonsterState monster in leaked)
        {
            state.ApplyBaseDamage(EconomyContent.MonsterLeakDamage);
            state.RemoveMonster(monster);
        }
    }

    private void UpdateTowers(GameState state, double deltaSeconds)
    {
        HashSet<MonsterState> killed = new();

        foreach (TowerState tower in state.Towers)
        {
            tower.TickCooldown(deltaSeconds);

            if (tower.CooldownRemainingSeconds > 0.0)
            {
                continue;
            }

            TowerLevelDefinition level = tower.CurrentLevelDefinition;

            MonsterState? target = SelectTarget(state, tower, level.Range);

            if (target is null)
            {
                continue;
            }

            tower.SetCooldown(level.CooldownSeconds);

            Point2D impactPoint = target.Position;

            foreach (MonsterState victim in GetDamageTargets(state, tower, level, impactPoint, target))
            {
                if (!victim.IsAlive)
                {
                    continue;
                }

                int damage = _damageCalculator.CalculateDamage(level, victim.Type);
                victim.ApplyDamage(damage);

                if (!victim.IsAlive)
                {
                    killed.Add(victim);
                }
            }
        }

        foreach (MonsterState monster in killed)
        {
            state.AddGold(monster.Type.GoldReward);
            state.RemoveMonster(monster);
        }
    }

    private static MonsterState? SelectTarget(
        GameState state,
        TowerState tower,
        double range)
    {
        MonsterState? bestTarget = null;

        foreach (MonsterState monster in state.Monsters)
        {
            if (!monster.IsAlive)
            {
                continue;
            }

            if (monster.Type.IsFlying && !tower.Type.CanTargetFlying)
            {
                continue;
            }

            double distance = tower.Position.DistanceTo(monster.Position);

            if (distance > range + ComparisonEpsilon)
            {
                continue;
            }

            if (bestTarget is null || monster.PathProgress > bestTarget.PathProgress)
            {
                bestTarget = monster;
            }
        }

        return bestTarget;
    }

    private static IReadOnlyList<MonsterState> GetDamageTargets(
        GameState state,
        TowerState tower,
        TowerLevelDefinition level,
        Point2D impactPoint,
        MonsterState primaryTarget)
    {
        List<MonsterState> targets = new();

        if (level.SplashRadius <= 0.0)
        {
            targets.Add(primaryTarget);
            return targets;
        }

        foreach (MonsterState monster in state.Monsters)
        {
            if (!monster.IsAlive)
            {
                continue;
            }

            if (monster.Type.IsFlying && !tower.Type.CanTargetFlying)
            {
                continue;
            }

            double distance = monster.Position.DistanceTo(impactPoint);

            if (distance <= level.SplashRadius + ComparisonEpsilon)
            {
                targets.Add(monster);
            }
        }

        return targets;
    }

    private sealed record SpawnRequest(string MonsterTypeId);
}