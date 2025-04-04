
namespace Settings.Global
{
    /// <summary>
    /// Use to configure specific events and remove them (it is useful for reuse).
    /// </summary>
    public interface IEventsConfigurable
    {
        public void ConfigureEventLinks();
        public void RemoveEventLinks();
    }
}
