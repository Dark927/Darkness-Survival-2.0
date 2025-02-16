
using UnityEngine;

namespace Characters.Common.Combat.Weapons.Data
{
    [CreateAssetMenu(fileName = "NewSwordAttackData", menuName = "Game/Combat/Data/Attacks/SwordAttackData")]
    public class SwordAttackData : WeaponAttackDataBase
    {
        // Note : There are some SerializedFields from the base class (basic attack settings).


        [SerializeField] private SwordAttackSettings _swordAttackSettings;

        public override AttackSettingsBase AttackSettings => _swordAttackSettings;
    }
}
