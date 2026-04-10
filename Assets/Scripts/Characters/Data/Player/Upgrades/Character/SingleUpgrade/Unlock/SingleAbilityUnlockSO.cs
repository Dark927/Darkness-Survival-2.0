
using Characters.Common.Abilities;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters.Player.Upgrades
{
    [CreateAssetMenu(fileName = "NewSingleAbilityUnlockData", menuName = "Game/Upgrades/Character Upgrades/Unlock/Single Ability Unlock Data")]
    public class SingleAbilityUnlockSO : SingleAbilityUnlockBaseSO
    {
        [SerializeField] private EntityAbilityData _abilityData;

        public override int AbilityID => _abilityData.ID;

        protected override string GetInfo(char sign)
        {
            return $"Open <color=orange>{_abilityData.Name}</color>";
        }

        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            target.AbilitiesHandler.GiveAbilityAsync(_abilityData);
        }
    }
}
