public interface ISettingsService
{
    GameSettings Load();
    void Save(GameSettings settings);
}