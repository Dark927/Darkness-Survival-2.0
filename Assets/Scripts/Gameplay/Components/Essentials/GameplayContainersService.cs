using Settings.Global;
using Zenject;

namespace Gameplay.Components
{
    /// <summary>
    /// A centralized service to manage all dynamically generated GameObject containers in the scene.
    /// Prevents hierarchy clutter and eliminates GameObject.Find() calls.
    /// </summary>
    public class GameplayContainersService : IService
    {
        private const string AutonomousAbilitiesName = "Abilities_Autonomous_Container";
        // You can add more constants here later (e.g., "Dropped_Loot_Container")

        private GameObjectsContainer _rootContainer;
        private GameObjectsContainer _autonomousAbilitiesContainer;

        [Inject]
        public GameplayContainersService(GameObjectsContainer rootContainer)
        {
            _rootContainer = rootContainer;
        }

        /// <summary>
        /// Lazy-loads the Master container for all autonomous abilities.
        /// </summary>
        public GameObjectsContainer GetAutonomousAbilitiesContainer()
        {
            if (_autonomousAbilitiesContainer == null)
            {
                _autonomousAbilitiesContainer = _rootContainer.GetChild(AutonomousAbilitiesName, enableForceCreate: true);
            }
            return _autonomousAbilitiesContainer;
        }

        /// <summary>
        /// Gets or creates a specific pool container for a weapon.
        /// </summary>
        public GameObjectsContainer GetAutonomousWeaponPoolContainer(string weaponName)
        {
            string innerContainerName = $"{weaponName}_Pool".Replace(" ", "_");
            return GetAutonomousAbilitiesContainer().GetChild(innerContainerName, enableForceCreate: true);
        }
    }
}
