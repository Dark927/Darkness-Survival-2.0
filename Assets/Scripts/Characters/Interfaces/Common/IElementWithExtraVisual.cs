
namespace Characters.Common
{
    /// <summary>
    /// Interface for classes that can activate some visual effects (like for weapons, abilities, etc.)
    /// </summary>
    public interface IElementWithExtraVisual
    {
        public bool IsSpecialVisualActive { get; }
        public void EnableSpecialVisual();
        public void DisableSpecialVisual();
    }
}
