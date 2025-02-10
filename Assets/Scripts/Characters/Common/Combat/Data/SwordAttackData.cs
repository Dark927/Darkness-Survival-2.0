
using UnityEngine;

namespace Characters.Common.Combat.Weapons.Data
{
    [CreateAssetMenu(fileName = "NewSwordAttackData", menuName = "Game/Characters/Data/Weapons/SwordAttackData")]
    public class SwordAttackData : WeaponAttackData
    {
        [SerializeField] private SwordAttackSettings _swordAttackSettings;

        public override AttackSettingsBase AttackSettings => _swordAttackSettings;
    }
}
