using Characters.Player.Attacks;
using Characters.Player.Data;
using Characters.Player.Weapons;
using Characters.Stats;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterBodyBase))]
    public class PlayerCharacterLogic : MonoBehaviour, ICharacterLogic
    {
        #region Fields

        [SerializeField] protected PlayerCharacterData _characterData;

        private bool _configured = false;

        private CharacterBodyBase _body;

        private CharacterWeaponsHolder _weaponHolder;
        private CharacterBasicAttack _attacks;

        #endregion


        #region Properties

        public CharacterBodyBase Body => _body;
        public CharacterBasicAttack BasicAttacks => _attacks;
        public CharacterBaseData Data => _characterData;
        public CharacterStats Stats => Data.Stats;
        public CharacterWeaponsHolder Weapons => _weaponHolder;

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

            _weaponHolder = new CharacterWeaponsHolder(this);
            _weaponHolder.Init();
            _weaponHolder.GiveMultipleWeapons(_characterData.BasicWeapons);

            SetBasicAttacks(new NeroBasicAttacks(this, _weaponHolder.ActiveWeapons));
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

        public virtual void Attack()
        {
            throw new System.NotImplementedException();
        }

        public void Enable()
        {
            if (!_configured)
            {
                SetReferences();
            }
        }

        protected void SetBasicAttacks(CharacterBasicAttack attacks)
        {
            _attacks = attacks;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }

    #endregion
}