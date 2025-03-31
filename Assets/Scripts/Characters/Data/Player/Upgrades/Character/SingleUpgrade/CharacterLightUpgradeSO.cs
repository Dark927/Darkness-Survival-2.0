using ModestTree;
using UnityEngine;
using Utilities.ErrorHandling;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewCharacterLightUpgradeSO", menuName = "Game/Upgrades/Character Upgrades/Single Upgrades/Light Upgrade Data")]
    public class CharacterLightUpgradeSO : SingleUniversalUpgradeSO<IUpgradableCharacterLogic>
    {
        [SerializeField, Range(0f, 100f)] private float _lightIntensityUpgradePercent = 0f;
        [SerializeField, Range(0f, 100f)] private float _lightRadiusUpgradePercent = 0f;

        protected override string GetInfo(char sign)
        {
            string infoText = string.Empty;

            if (_lightIntensityUpgradePercent > 0f)
            {
                infoText = $"<color=white>Luminous vision : {sign}{_lightIntensityUpgradePercent}%</color>";
            }

            if (_lightRadiusUpgradePercent > 0f)
            {
                if (!infoText.IsEmpty())
                {
                    infoText += '\n';
                }
                infoText += $"<color=white>Sight distance : {sign}{_lightRadiusUpgradePercent}%</color>";
            }

            if (infoText.IsEmpty())
            {
                ErrorLogger.LogWarning($"Light upgrade | Wrong values (radius : {_lightRadiusUpgradePercent}; intensity : {_lightIntensityUpgradePercent} | {this}");
            }

            return infoText;
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.ApplyLightIntensityUpgrade(1 + (_lightIntensityUpgradePercent / 100f));
            target.ApplyLightRadiusUpgrade(1 + (_lightRadiusUpgradePercent / 100f));
        }

        public override void ApplyDowngrade(IUpgradableCharacterLogic target)
        {
            target.ApplyLightIntensityUpgrade(1 - (_lightIntensityUpgradePercent / 100f));
            target.ApplyLightRadiusUpgrade(1 - (_lightRadiusUpgradePercent / 100f));
        }
    }
}
