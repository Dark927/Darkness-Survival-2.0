using System;
using System.Collections.Generic;
using Characters.Common.Abilities;
using Characters.Common.Combat.Weapons;
using Characters.Common.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters.Common
{
    public abstract class AttackableEntityLogicBase : MonoBehaviour, IAttackableEntityLogic
    {
        #region Fields

        public event EventHandler<IEntityDynamicLogic> OnEnemyKilled;

        protected AttackableCharacterData _entityData;
        private IEntityPhysicsBody _body;

        private EntityWeaponAbilitiesHandler _weaponHandler;
        private EntityPassiveAbilitiesHandler _abilitiesHandler;

        #endregion


        #region Properties

        public IEntityPhysicsBody Body => _body;
        public CharacterStats Stats => _entityData.Stats;
        public EntityInfo Info => _entityData.CommonInfo;
        public EntityWeaponAbilitiesHandler WeaponsHandler => _weaponHandler;
        public EntityPassiveAbilitiesHandler AbilitiesHandler => _abilitiesHandler;


        #endregion


        #region Methods

        #region Init

        public virtual void Initialize(IEntityData data)
        {
            _entityData = data as AttackableCharacterData;
            InitBody();
            _abilitiesHandler = new EntityPassiveAbilitiesHandler(this);
            InitWeaponsAsync();
            SetReferences();
        }

        private void InitBody()
        {
            _body = GetComponent<IEntityPhysicsBody>();
            _body.Initialize();
        }

        private void InitWeaponsAsync()
        {
            if (_entityData.WeaponsSetData == null)
            {
                return;
            }

            _weaponHandler = new EntityWeaponAbilitiesHandler(this, _entityData.WeaponsSetData.ContainerName);

            _weaponHandler.InitBasicWeaponsAsync(_entityData.WeaponsSetData.BasicWeapons)
                .ContinueWith(() =>
                {
                    _weaponHandler.SetBasicAttacks(GetBasicAttacks());
                    _weaponHandler.InitBasicAttacks();
                    _weaponHandler.SetWeaponsDamageMultiplier(_entityData.DamageMultiplier);
                }).Forget();

            _weaponHandler.OnNewWeaponGiven += ListenNewWeaponGiven;
        }

        protected virtual BasicAttack GetBasicAttacks()
        {
            return new BasicAttack(Body, new List<IWeapon>());
        }

        public virtual void Dispose()
        {
            WeaponsHandler.OnNewWeaponGiven -= ListenNewWeaponGiven;
            WeaponsHandler.Dispose();
            Body.Dispose();
        }

        public virtual void ConfigureEventLinks()
        {
            Body?.ConfigureEventLinks();
            WeaponsHandler?.ConfigureEventLinks();
        }

        public virtual void RemoveEventLinks()
        {
            Body?.RemoveEventLinks();
        }

        protected virtual void SetReferences()
        {

        }


        private void OnDestroy()
        {
            Dispose();
        }

        #endregion


        public void ResetState()
        {
            Body.ResetState();
        }

        public void ApplyStun(int durationMs)
        {
            Body.Movement.Block(durationMs);
        }

        public virtual void ListenNewWeaponGiven(object sender, IWeapon weapon)
        {

        }

        public void NotifyEnemyKilled(IEntityDynamicLogic killedEnemy)
        {
            OnEnemyKilled?.Invoke(this, killedEnemy);
        }

        #endregion

    }
}
