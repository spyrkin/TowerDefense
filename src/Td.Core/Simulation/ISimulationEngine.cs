/// <summary>
/// Контракт игрового симуляционного движка.
/// Движок обновляет состояние игры на фиксированный шаг симуляции.
/// </summary>
public interface ISimulationEngine
{
    /// <summary>
    /// Запускает следующую волну, если это допустимо текущим состоянием игры.
    /// </summary>
    void StartNextWave(GameState state);

    /// <summary>
    /// Обновляет симуляцию на один фиксированный шаг.
    /// </summary>
    /// <param name="state">Текущее состояние игры.</param>
    /// <param name="deltaSeconds">Шаг времени в симуляционных секундах.</param>
    void Update(GameState state, double deltaSeconds);
}