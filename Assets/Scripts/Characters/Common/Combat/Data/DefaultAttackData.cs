using UnityEngine;

namespace Characters.Common.Combat.Weapons.Data
{
    [CreateAssetMenu(fileName = "NewDefaultAttackData", menuName = "Game/Combat/Data/Attacks/DefaultAttackData")]

    public class DefaultAttackData : WeaponAttackData
    {
        [SerializeField] private AttackSettingsBase _defaultAttackSettings;
        public override AttackSettingsBase AttackSettings => _defaultAttackSettings;
    }
}
