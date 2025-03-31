
using Characters.Common.Visual;
using UnityEngine;
using Utilities.ErrorHandling;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterTrailUpgradeSO", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Special/Trail Upgrade Data")]
    public class CharacterTrailUpgradeSO : SingleUpgradeBaseSO<IUpgradableCharacterLogic>
    {
        [SerializeField] private GameObject _trailVisualPrefab;
        [SerializeField, TextArea] private string _description;


        protected override string GetInfo(char sign)
        {
            return _description;
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            if (_trailVisualPrefab == null)
            {
                ErrorLogger.LogWarning("Prefab is null. Cannot create custom trail visual part. -" + this);
                return;
            }

            // Check if the prefab has any component that implements IEntityCustomVisualPart
            IEntityCustomVisualPart customVisual = _trailVisualPrefab.GetComponent<IEntityCustomVisualPart>();
            if (customVisual == null)
            {

                ErrorLogger.LogWarning("Prefab does not have any component implementing ICustomVisual. Visual part not created. -" + this);
                return;
            }

            GameObject visualInstance = Instantiate(_trailVisualPrefab);
            visualInstance.name = _trailVisualPrefab.name;
            var visualPart = visualInstance.GetComponent<IEntityCustomVisualPart>();
            visualPart.Initialize(target.Body);
            target.Body.Visual.GiveCustomVisualPart(visualPart);
        }
    }
}
