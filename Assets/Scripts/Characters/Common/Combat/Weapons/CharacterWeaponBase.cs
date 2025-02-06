
using Characters.Common.Combat.Weapons.Data;
using System;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public abstract class CharacterWeaponBase : MonoBehaviour, IWeapon, IDisposable
    {
        [SerializeField] private WeaponAttackData _weaponAttackData;

        public WeaponAttackData AttackData => _weaponAttackData;

        public virtual void Dispose()
        {

        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
