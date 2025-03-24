using System;
using Characters.Common.Combat;
using Characters.Common.Combat.Weapons;
using Characters.Interfaces;
using Characters.Stats;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters.Common
{
    public abstract class AttackableEntityLogic : MonoBehaviour, IEntityDynamicLogic, IAttackable<BasicAttack>, IDisposable
    {
        #region Fields

        public event Action<BasicAttack> OnBasicAttacksReady;

        private bool _configured = false;

        protected AttackableCharacterData _entityData;
        private EntityWeaponsHolder _weaponHolder;
        private BasicAttack _attacks;
        private IEntityPhysicsBody _body;

        #endregion


        #region Properties

        public IEntityPhysicsBody Body => _body;
        public IEntityData Data => _entityData;
        public EntityWeaponsHolder Weapons => _weaponHolder;
        public BasicAttack BasicAttacks => _attacks;
        public CharacterStats Stats => Data.Stats;

        #endregion


        #region Methods

        #region Init

        public virtual void Initialize(IEntityData data)
        {
            _entityData = data as AttackableCharacterData;
            SetComponents();
            InitBody();
            InitWeaponsAsync();
            SetReferences();
        }

        protected virtual void SetComponents()
        {
            if (_entityData.WeaponsSetData != null)
            {
                _weaponHolder = new EntityWeaponsHolder(this, _entityData.WeaponsSetData.ContainerName);
            }
        }

        private void InitBody()
        {
            _body = GetComponent<IEntityPhysicsBody>();
            _body.Initialize();
        }

        private void InitWeaponsAsync()
        {
            if (Weapons == null)
            {
                return;
            }

            Weapons.Initialize();

            Weapons.GiveMultipleFeaturesAsync(_entityData.WeaponsSetData.BasicWeapons)
                .ContinueWith(ConfigureWeapons)
                .ContinueWith(InitBasicAttacks)
                .Forget();

            Weapons.OnNewWeaponGiven += ListenNewWeaponGiven;
        }

        protected virtual void ConfigureWeapons()
        {
            foreach (var weapon in Weapons.ActiveOnesDict.Values)
            {
                weapon.SetCharacterDamageMultiplier(_entityData.DamageMultiplier);
            }
        }

        protected virtual void InitBasicAttacks()
        {
            if (BasicAttacks != null)
            {
                BasicAttacks.Init();
                OnBasicAttacksReady?.Invoke(BasicAttacks);
            }
        }

        protected virtual void SetReferences()
        {
            _configured = true;
        }

        public virtual void Dispose()
        {
            if (Weapons != null)
            {
                Weapons.Dispose();
                Weapons.OnNewWeaponGiven -= ListenNewWeaponGiven;
            }

            _configured = false;
        }

        public virtual void ConfigureEventLinks()
        {
            Body?.ConfigureEventLinks();
        }

        public virtual void RemoveEventLinks()
        {
            Body?.RemoveEventLinks();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion

        public virtual void PerformBasicAttack(BasicAttack.LocalType type)
        {
            BasicAttacks?.TryPerformAttack(type);
        }

        public void Enable()
        {
            if (!_configured)
            {
                SetReferences();
            }
        }

        public void ResetState()
        {
            Body.ResetState();
        }

        protected void SetBasicAttacks(BasicAttack attacks)
        {
            _attacks = attacks;
        }

        public void ApplyStun(int durationMs)
        {
            Body.Movement.Block(durationMs);
        }

        public virtual void ListenNewWeaponGiven(IWeapon weapon)
        {

        }

        #endregion

    }
}
