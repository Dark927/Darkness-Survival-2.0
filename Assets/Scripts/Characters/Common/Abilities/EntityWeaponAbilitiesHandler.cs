using System;
using System.Collections.Generic;
using Characters.Common.Combat;
using Characters.Common.Combat.Weapons;
using Characters.Common.Combat.Weapons.Data;
using Characters.Common.Features;
using Cysharp.Threading.Tasks;
using Settings.Global;
using Utilities.ErrorHandling;

namespace Characters.Common.Abilities
{
    public class EntityWeaponAbilitiesHandler : IFeaturesHolderProvider<IWeapon>, IEventsConfigurable
    {
        #region Events

        public event EventHandler<IWeapon> OnNewWeaponGiven;

        #endregion


        #region Fields

        public event Action<BasicAttack> OnBasicAttacksReady;

        private EntityWeaponsHolder _weaponsHolder;
        private BasicAttack _attacks;

        private IAttackableEntityLogic _ownerLogic;

        #endregion


        #region Properties

        public BasicAttack BasicAttacks => _attacks;
        public IEnumerable<IWeapon> ActiveOnes => _weaponsHolder.ActiveOnesDict.Values;

        #endregion


        #region Methods 

        #region Init

        public EntityWeaponAbilitiesHandler(IAttackableEntityLogic owner, string weaponsContainerName = null)
        {
            _ownerLogic = owner;
            _weaponsHolder = new EntityWeaponsHolder(owner, weaponsContainerName);
            _weaponsHolder.OnNewWeaponGiven += ListenHolderNewWeaponGiven;
        }

        public UniTask InitBasicWeaponsAsync(List<EntityWeaponData> basicWeaponsData)
        {
            if (_weaponsHolder == null)
            {
                ErrorLogger.LogWarning($"{this} - field of type {nameof(EntityWeaponsHolder)} is null! Can not init basic weapons");
                return default;
            }

            _weaponsHolder.Initialize();
            return _weaponsHolder.GiveMultipleFeaturesAsync(basicWeaponsData);
        }

        public void ConfigureEventLinks()
        {
            ConfigureBasicAttacksEvents();
        }

        public void RemoveEventLinks()
        {
            BasicAttacks?.RemoveEventLinks();
        }

        #region Basic Attacks Configuration

        private void ConfigureBasicAttacksEvents()
        {
            if (BasicAttacks != null)
            {
                BasicAttacks.ConfigureEventLinks();
                return;
            }

            OnBasicAttacksReady += ConfigureBasicAttacksEventListener;
        }

        public virtual void InitBasicAttacks()
        {
            if (BasicAttacks != null)
            {
                BasicAttacks.Init();
                OnBasicAttacksReady?.Invoke(BasicAttacks);
            }
        }

        private void ConfigureBasicAttacksEventListener(BasicAttack attack)
        {
            OnBasicAttacksReady -= ConfigureBasicAttacksEventListener;
            attack.ConfigureEventLinks();
        }

        #endregion

        public void Dispose()
        {
            if (_weaponsHolder == null)
            {
                return;
            }

            _weaponsHolder.OnNewWeaponGiven -= ListenHolderNewWeaponGiven;
            _weaponsHolder.Dispose();

            OnBasicAttacksReady -= ConfigureBasicAttacksEventListener;
        }

        #endregion

        public bool TryGetFeatureByID(int weaponID, out IWeapon weapon)
        {
            weapon = null;
            return (_weaponsHolder != null) && _weaponsHolder.ActiveOnesDict.TryGetValue(weaponID, out weapon);
        }

        public UniTask GiveWeaponAsync(EntityWeaponData weaponData)
        {
            return _weaponsHolder.GiveAsync(weaponData);
        }

        public void TryPerformBasicAttack(BasicAttack.LocalType type)
        {
            BasicAttacks?.TryPerformBasicAttack(type);
        }

        public virtual void SetWeaponsDamageMultiplier(float damageMultiplier)
        {
            foreach (var weapon in _weaponsHolder.ActiveOnesDict.Values)
            {
                weapon.SetCharacterDamageMultiplier(damageMultiplier);
            }
        }

        public void SetBasicAttacks(BasicAttack attacks)
        {
            _attacks = attacks;
        }

        private void ListenHolderNewWeaponGiven(IWeapon weapon)
        {
            OnNewWeaponGiven?.Invoke(this, weapon);
        }

        #endregion
    }
}
