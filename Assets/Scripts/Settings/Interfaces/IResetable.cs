

namespace Settings.Global
{
    /// <summary>
    /// Interface for entities that can reset to their original state.
    /// </summary>
    public interface IResetable
    {
        public void ResetState();
    }
}
