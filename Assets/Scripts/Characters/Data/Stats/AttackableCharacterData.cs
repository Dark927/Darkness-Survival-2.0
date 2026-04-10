

using Characters.Common.Combat.Weapons.Data;
using UnityEngine;

namespace Characters.Common.Settings
{
    public class AttackableCharacterData : EntityBaseData
    {
        [Space, Header("Weapons Settings")]

        [SerializeField] private float _damageMultiplier = 1f;

        [SerializeField] private WeaponSetData _weaponSetData;

        public float DamageMultiplier => _damageMultiplier;
        public WeaponSetData WeaponsSetData => _weaponSetData;
    }
}
