using Characters.Common;
using Characters.Interfaces;
using Settings.Global;
using System;
using Cysharp.Threading.Tasks;
using Characters.Common.Combat.Weapons;
using UnityEngine.InputSystem;
using UnityEngine;
using Characters.Stats;

namespace Characters.Player
{
    public class PlayerCharacterController : EntityControllerBase
    {
        #region Fields 

        private PlayerInput _input;
        [SerializeField] private EntityBaseData _characterData;

        #endregion


        #region Properties

        public ICharacterLogic CharacterLogic => EntityLogic as ICharacterLogic;

        #endregion


        #region Events

        public event Action OnCharacterDeathEnd;

        #endregion


        #region Methods

        #region Init 

        private void Awake()
        {
            Initialize(_characterData);
        }

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);
            InitInput();
            ConfigureCharacter(data);
            InitFeaturesAsync().Forget();
        }

        protected override void Start()
        {
            base.Start();
            ServiceLocator.Current.Get<PlayerService>()?.AddPlayer(this);

            ConfigureEventLinks();
        }

        private void ConfigureCharacter(IEntityData data)
        {
            EntityLogic.Initialize(data);
            EntityLogic.ConfigureEventLinks();
        }

        public override void ConfigureEventLinks()
        {
            base.ConfigureEventLinks();

            PlayerCharacterVisual visual = (EntityLogic.Body.Visual as PlayerCharacterVisual);

            EntityLogic.ConfigureEventLinks();
            EntityLogic.Body.OnBodyDies += PlayerCharacterDies;
            EntityLogic.Body.OnBodyDiedCompletely += NotifyCharacterCompletelyDied;
        }

        private void InitInput()
        {
            _input = GetComponent<PlayerInput>();
        }

        public override void RemoveEventLinks()
        {
            base.RemoveEventLinks();
            EntityLogic.RemoveEventLinks();
            EntityLogic.Body.OnBodyDies -= PlayerCharacterDies;
            EntityLogic.Body.OnBodyDiedCompletely -= NotifyCharacterCompletelyDied;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        #endregion

        public void OnMove(InputAction.CallbackContext context)
        {
            if (CharacterLogic.Body.Movement == null)
            {
                return;
            }

            if (context.performed)
            {
                Vector2 direction = context.ReadValue<Vector2>();
                CharacterLogic.Body.Movement.MoveAsync(direction).Forget();
            }

            if (context.canceled)
            {
                CharacterLogic.Body.Movement.Stop();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                int contextValue = (int)context.ReadValue<float>();
                CharacterLogic.PerformBasicAttack((BasicAttack.LocalType)contextValue);
            }
        }

        private void PlayerCharacterDies()
        {
            CharacterLogic.Body.Physics.SetStatic();
            _input.DeactivateInput();
        }

        private void NotifyCharacterCompletelyDied()
        {
            OnCharacterDeathEnd?.Invoke();
        }

        #endregion
    }
}
