
using UnityEngine;

/// <summary>
/// Base class for ScriptableObjects that need a public description field.
/// </summary>

namespace Settings
{
    public class DescriptionBaseData : ScriptableObject
    {
        [TextArea] public string Description;
    }
}