

namespace Settings.Global
{
    /// <summary>
    /// Represents an interface for elements that can be cleaned. 
    /// This is used for objects that need to reset their state or remove temporary data 
    /// but do not require full disposal (e.g., memory or unmanaged resource cleanup).
    /// The <c>Clean()</c> method allows the object to reset its fields and remove any accumulated garbage 
    /// without being disposed of entirely, making it ready for reuse.
    /// </summary>
    internal interface ICleanable
    {

        /// <summary>
        /// cleans the object's state and removes any temporary data or garbage.
        /// </summary>
        public void Clean();
    }
}
