using Gameplay.Components;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// A global wrapper for the autonomous abilities container.
    /// Should be registered as a Singleton in Zenject
    /// </summary>
    public class AutonomousAbilitiesContainer
    {
        public const string ContainerDefaultName = "Abilities_Autonomous_Container";
        private GameObjectsContainer _masterContainer;

        public GameObjectsContainer MasterContainer => _masterContainer;

        public AutonomousAbilitiesContainer(GameObjectsContainer container = null, string preferredContainerName = ContainerDefaultName)
        {
            if (container != null)
            {
                _masterContainer = container;
                if (preferredContainerName != ContainerDefaultName)
                {
                    _masterContainer.name = preferredContainerName;
                }
            }
            else
            {
                // Creates it in the active scene (GameplayEssentials)
                GameObject obj = new GameObject(preferredContainerName, typeof(GameObjectsContainer));
                _masterContainer = obj.GetComponent<GameObjectsContainer>();
            }
        }

        /// <summary>
        /// Automatically finds or creates a specific sub-container for a weapon.
        /// </summary>
        public GameObjectsContainer GetWeaponPoolContainer(string weaponName)
        {
            string innerContainerName = $"{weaponName}_Pool".Replace(" ", "_");
            return _masterContainer.GetChild(innerContainerName, enableForceCreate: true);
        }
    }
}
