
namespace Settings.Global
{
    public interface ISettingsStorage : IInitializable
    {
        public GameSettingsData Data { get; }
        public bool IsLoaded { get; }
        public void SaveAllSettings();
    }
}
