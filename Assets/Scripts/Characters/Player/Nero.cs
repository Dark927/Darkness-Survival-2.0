using Characters.Health;
using Characters.Player.Animation;
using Characters.Player.Attacks;
using Characters.Player.Data;
using Characters.Stats;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterBody))]
    public class Nero : MonoBehaviour, ICharacterLogic
    {
        #region Fields

        [SerializeField] private PlayerCharacterData _characterData;

        private bool _configured = false;
        private CharacterBody _body;
        private CharacterBasicAttack _attacks;
        private CharacterAnimatorController _animatorController;

        #endregion


        #region Properties

        public CharacterBody Body => _body;
        public CharacterBasicAttack BasicAttacks => _attacks;
        public CharacterStats Stats => _characterData.Stats;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            InitComponents();
            InitBasicAttacks();
        }

        private void Start()
        {
            SetReferences();
        }

        private void InitComponents()
        {
            _body = GetComponent<CharacterBody>();

        }

        private void InitBasicAttacks()
        {
            _attacks = new CharacterBasicAttack();
        }

        private void SetReferences()
        {
            // ToDo : Move this logic to the another place.
            _animatorController = _body.Visual.GetAnimatorController() as CharacterAnimatorController;
            _attacks.OnFastAttack += _animatorController.TriggerFastAttack;
            _attacks.OnHeavyAttack += _animatorController.TriggerHeavyAttack;



            _configured = true;
        }

        #endregion

        public void Attack()
        {
            throw new System.NotImplementedException();
        }

        private void OnEnable()
        {
            if (!_configured)
            {
                SetReferences();
            }
        }

        private void OnDisable()
        {
            ClearReferences();
        }

        private void ClearReferences()
        {
            _attacks.OnFastAttack -= _animatorController.TriggerFastAttack;
            _attacks.OnHeavyAttack -= _animatorController.TriggerHeavyAttack;
            _animatorController = null;

            _configured = false;
        }

        #endregion
    }
}

