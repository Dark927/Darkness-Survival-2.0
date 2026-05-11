using System;
using System.Collections.Generic;
using Characters.Common.Abilities;
using Characters.Common.Combat;
using Characters.Common.Combat.Weapons;
using Characters.Common.Settings;
using Characters.Common.Statuses;
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
        private IEntityStatusLogic _statusLogic;

        #endregion


        #region Properties

        public IEntityPhysicsBody Body => _body;
        public CharacterStats Stats => _entityData.Stats;
        public EntityInfo Info => _entityData.CommonInfo;
        public EntityWeaponAbilitiesHandler WeaponsHandler => _weaponHandler;
        public EntityPassiveAbilitiesHandler AbilitiesHandler => _abilitiesHandler;
        public IEntityStatusLogic Status => _statusLogic;


        #endregion


        #region Methods

        #region Init

        public virtual void Initialize(IEntityData data)
        {
            _entityData = data as AttackableCharacterData;
            InitBody();
            _abilitiesHandler = new EntityPassiveAbilitiesHandler(this);
            _statusLogic = new EntityStatusLogic(this);
            InitWeaponsAsync();
            SetReferences();
        }

        protected virtual void Update()
        {
            _statusLogic?.UpdateTimers();
        }

        private void InitBody()
        {
            _body = GetComponent<IEntityPhysicsBody>();
            _body.Initialize();

            DamageableType damageableType = _entityData.CommonInfo.EntityType switch
            {
                EntityType.Enemy => DamageableType.Enemy,
                EntityType.Player => DamageableType.Player,
                _ => DamageableType.Undefined,
            };

            _body.SetDamageableType(damageableType);
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

            if (Body?.Physics != null)
            {
                Body.Physics.OnStunRequested += HandleStunRequested;
            }
        }

        public virtual void RemoveEventLinks()
        {
            Body?.RemoveEventLinks();
            WeaponsHandler?.RemoveEventLinks();

            if (Body?.Physics != null)
            {
                Body.Physics.OnStunRequested -= HandleStunRequested;
            }
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
            _statusLogic?.ClearAll();
            Body.ResetState();
        }

        private void HandleStunRequested(int durationMs)
        {
            // This is where the logic determines WHAT “being stunned” means.
            ApplyStun(durationMs);

            // ToDo : we can add here:
            // *Cancel the attack*
            // *Play the animation*
        }

        public void ApplyStun(int durationMs)
        {
            // Convert milliseconds to seconds and pass it
            Status.Apply(new StunStatusEffect(durationMs / 1000f));
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
