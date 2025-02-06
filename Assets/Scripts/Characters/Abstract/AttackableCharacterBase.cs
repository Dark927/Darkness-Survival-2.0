using Characters.Common.Combat.Weapons;
using Characters.Interfaces;
using Characters.Player.Weapons;
using Characters.Stats;
using UnityEngine;

namespace Characters.Common
{
    public abstract class AttackableCharacterBase : MonoBehaviour, IEntityLogic, IAttackable<BasicAttack>
    {
        #region Fields

        private bool _configured = false;

        [SerializeField] protected AttackableCharacterData _characterData;
        private CharacterWeaponsHolder _weaponHolder;
        private BasicAttack _attacks;
        private CharacterBodyBase _body;

        #endregion


        #region Properties

        public CharacterBodyBase Body => _body;
        public CharacterBaseData Data => _characterData;
        public CharacterWeaponsHolder Weapons => _weaponHolder;
        public BasicAttack BasicAttacks => _attacks;
        public CharacterStats Stats => Data.Stats;

        #endregion


        #region Methods

        #region Init

        protected virtual void Awake()
        {
            InitComponents();
        }

        protected virtual void Start()
        {
            InitBasicAttacks();
            SetReferences();
        }

        protected virtual void InitComponents()
        {
            _body = GetComponent<CharacterBodyBase>();
            _weaponHolder = new CharacterWeaponsHolder(this, _characterData.WeaponsSetData.ContainerName);
            _weaponHolder.Init();
            _weaponHolder.GiveMultipleWeapons(_characterData.WeaponsSetData.BasicWeapons);
        }

        protected virtual void InitBasicAttacks()
        {
            BasicAttacks.Init();
        }

        protected virtual void SetReferences()
        {
            _configured = true;
        }

        protected virtual void Dispose()
        {
            _configured = false;
        }

        #endregion

        public void Enable()
        {
            if (!_configured)
            {
                SetReferences();
            }
        }

        protected void SetBasicAttacks(BasicAttack attacks)
        {
            _attacks = attacks;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion

    }
}
