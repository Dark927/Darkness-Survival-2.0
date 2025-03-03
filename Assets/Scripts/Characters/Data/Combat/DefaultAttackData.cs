using UnityEngine;

namespace Characters.Common.Combat.Weapons.Data
{
    [CreateAssetMenu(fileName = "NewDefaultAttackData", menuName = "Game/Combat/Data/Attacks/DefaultAttackData")]

    public class DefaultAttackData : WeaponAttackDataBase
    {
        [SerializeField] private BasicAttackSettings _defaultAttackSettings;
        public override IAttackSettings Settings => _defaultAttackSettings;
    }
}
