public sealed class GameState
{
    public required MapDefinition Map { get; init; }

    public GamePhase Phase { get; set; } = GamePhase.MainMenu;

    public GameSpeed Speed { get; set; } = GameSpeed.X1;

    public int Gold { get; set; }

    public int Lives { get; set; } = MechanicsLimits.BaseLives;

    public int CurrentWave { get; set; }

    public int TotalWaves { get; set; }

    public IList<TowerState> Towers { get; } = new List<TowerState>();

    public IList<MonsterState> Monsters { get; } = new List<MonsterState>();
}