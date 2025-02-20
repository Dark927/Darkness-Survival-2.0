using Characters.Common.Combat.Weapons;
using Characters.Interfaces;
using Characters.Player.Weapons;
using Characters.Stats;
using Cysharp.Threading.Tasks;
using System;
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
            Weapons?.Initialize();

            Weapons?.GiveMultipleFeaturesAsync(_entityData.WeaponsSetData.BasicWeapons)
                .ContinueWith(InitBasicAttacks)
                .Forget();
        }

        protected virtual void InitBasicAttacks()
        {
            BasicAttacks?.Init();

            if (BasicAttacks != null)
            {
                OnBasicAttacksReady?.Invoke(_attacks);
            }
        }

        protected virtual void SetReferences()
        {
            _configured = true;
        }

        public virtual void Dispose()
        {
            Weapons?.Dispose();
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

        #endregion

    }
}
