public interface IRandomWaveGenerator
{
    int RollTotalWaveCount(Random random);

    WaveDefinition GenerateWave(
        int waveNumber,
        int totalWaves,
        Random random);
}