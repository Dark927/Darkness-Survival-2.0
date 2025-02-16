using Characters.Common;
using Characters.Interfaces;
using Settings.Global;
using System;
using Cysharp.Threading.Tasks;
using Characters.Common.Combat.Weapons;
using UnityEngine.InputSystem;
using UnityEngine;
using Characters.Player.Controls;

namespace Characters.Player
{
    public class PlayerCharacterController : EntityControllerBase
    {
        #region Fields 

        private PlayerInput _input;

        #endregion


        #region Properties

        public ICharacterLogic Character => EntityLogic as ICharacterLogic;

        #endregion


        #region Events

        public event Action OnCharacterDeathEnd;

        #endregion


        #region Methods

        #region Init 

        protected override void Awake()
        {
            base.Awake();
            InitInput();
            ConfigureCharacter();
            InitFeaturesAsync().Forget();
        }

        protected override void Start()
        {
            base.Start();
            ServiceLocator.Current.Get<PlayerManager>()?.AddPlayer(this);

            ConfigureEventLinks();
        }

        private void ConfigureCharacter()
        {
            EntityLogic.Initialize();
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
            if (Character.Body.Movement == null)
            {
                return;
            }

            if (context.performed)
            {
                Vector2 direction = context.ReadValue<Vector2>();
                Character.Body.Movement.MoveAsync(direction).Forget();

                if (!Character.Body.Movement.IsBlocked)
                {
                    Character.Body.View.LookForward(direction);
                }
            }

            if (context.canceled)
            {
                Character.Body.Movement.Stop();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                int contextValue = (int)context.ReadValue<float>();
                Character.PerformBasicAttack((BasicAttack.LocalType)contextValue);
            }
        }

        private void PlayerCharacterDies()
        {
            _input.DeactivateInput();
        }

        private void NotifyCharacterCompletelyDied()
        {
            OnCharacterDeathEnd?.Invoke();
        }

        #endregion
    }
}
