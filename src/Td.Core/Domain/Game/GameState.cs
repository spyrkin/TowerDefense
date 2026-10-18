

/// <summary>
/// Корень агрегата игрового состояния. 
/// Инкапсулирует все мутации и гарантирует соблюдение бизнес-инвариантов.
/// </summary>
public sealed class GameState
{
    private readonly List<TowerState> _towers = new();
    private readonly List<MonsterState> _monsters = new();

    public required MapDefinition Map { get; init; }
    
    // TotalWaves не должен меняться в процессе матча
    public required int TotalWaves { get; init; } 

    public GamePhase Phase { get; private set; } = GamePhase.MainMenu;
    public GameSpeed Speed { get; set; } = GameSpeed.X1;
    
    public int Gold { get; private set; }
    public int Lives { get; private set; } = MechanicsLimits.BaseLives;
    
    public int CurrentWave { get; private set; }

    // Read-only view для предотвращения внешней мутации
    public IReadOnlyList<TowerState> Towers => _towers;
    public IReadOnlyList<MonsterState> Monsters => _monsters;

    // --- Явные триггеры и мутаторы состояния ---

	public void StartGame()
	{
		if (Phase is not (GamePhase.MainMenu or GamePhase.MapSelect))
			throw new InvalidOperationException($"Cannot start game from phase {Phase}.");

		_towers.Clear();
		_monsters.Clear();

		Gold = EconomyContent.StartingGold;
		Lives = MechanicsLimits.BaseLives;
		CurrentWave = 0;
		Phase = GamePhase.Playing;
	}

    public void PauseGame()
    {
        if (Phase != GamePhase.Playing) return;
        Phase = GamePhase.Paused;
    }

    public void ResumeGame()
    {
        if (Phase != GamePhase.Paused) return;
        Phase = GamePhase.Playing;
    }

    public void StartNextWave()
    {
        if (Phase != GamePhase.Playing)
            throw new InvalidOperationException("Cannot start wave outside of Playing phase.");
        if (CurrentWave >= TotalWaves)
            throw new InvalidOperationException("All waves have already been spawned.");
            
        CurrentWave++;
        EvaluateVictoryCondition();
    }

    public void AddTower(TowerState tower)
    {
        if (Phase != GamePhase.Playing)
            throw new InvalidOperationException("Cannot add towers outside of Playing phase.");
        
        _towers.Add(tower);
    }

    public void RemoveTower(TowerState tower)
    {
        _towers.Remove(tower);
    }

    public void AddMonster(MonsterState monster)
    {
        if (Phase != GamePhase.Playing)
            throw new InvalidOperationException("Cannot spawn monsters outside of Playing phase.");
        
        _monsters.Add(monster);
    }

    public void RemoveMonster(MonsterState monster)
    {
        _monsters.Remove(monster);
        EvaluateVictoryCondition();
    }

    public void ApplyBaseDamage(int damage)
    {
        if (damage <= 0) return;
        if (Phase != GamePhase.Playing) return;

        Lives -= damage;
        if (Lives <= 0)
        {
            Lives = 0;
            Phase = GamePhase.Defeat;
        }
    }

    public void AddGold(int amount)
    {
        if (amount < 0) throw new ArgumentException("Gold amount cannot be negative.");
        Gold += amount;
    }

    public void SpendGold(int amount)
    {
        if (amount < 0) throw new ArgumentException("Amount cannot be negative.");
        if (Gold < amount) throw new InvalidOperationException("Insufficient gold.");
        
        Gold -= amount;
    }

    // --- Приватные методы оценки инвариантов ---

    private void EvaluateVictoryCondition()
    {
        if (Phase != GamePhase.Playing) return;

        bool allWavesSpawned = CurrentWave >= TotalWaves;
        // Проверяем, что все монстры либо удалены, либо мертвы (защита от гонок с очисткой списка)
        bool allMonstersDefeated = _monsters.Count == 0 || !_monsters.Any(m => m.IsAlive);

        if (allWavesSpawned && allMonstersDefeated)
        {
            Phase = GamePhase.Victory;
        }
    }
}