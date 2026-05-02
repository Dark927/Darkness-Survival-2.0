using System;
using Characters.Common;
using Characters.Common.Combat.Weapons;
using Characters.Common.Settings;
using Characters.Player.Controls;
using Characters.Player.Upgrades;
using Cysharp.Threading.Tasks;
using Gameplay.Components;
using Settings.Global;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class PlayerCharacterController : EntityControllerBase
    {
        #region Fields 

        private bool _canAttack = true;
        [SerializeField] private EntityBaseData _characterData;
        private CharacterInputHandler _input;

        #endregion

        #region Properties

        public ICharacterLogic CharacterLogic => EntityLogic as ICharacterLogic;
        public CharacterInputHandler Input => _input;

        #endregion

        #region Events

        public event Action<PlayerCharacterController> OnCharacterDeathEnd;
        public event Action<PlayerCharacterController> OnCharacterDies;
        public event Action<PlayerCharacterController> OnCharacterFinishesIntro;

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
            EntityLogic.Body.OnBodyDies += RaiseCharacterDies;
            EntityLogic.Body.OnBodyDiedCompletely += RaiseCharacterCompletelyDied;

            // subscribe to both Performed (key down) and Canceled (key up) for movement
            _input.SubscribeOnActionPerformed(InputType.Movement, OnMovement);
            _input.SubscribeOnActionCanceled(InputType.Movement, OnMovement);

            // Subscribe to separated attacks
            _input.SubscribeOnActionPerformed(InputType.FastAttack, OnFastAttack);
            _input.SubscribeOnActionPerformed(InputType.HeavyAttack, OnHeavyAttack);

            // Subscribe to upgrades events

            if (CharacterLogic is IUpgradableCharacterLogic upgradableCharacterLogic)
            {
                upgradableCharacterLogic.OnIntroUpgradesReceived += RaiseCharacterFinishesIntro;
            }
        }

        private void InitInput()
        {
            // grabs the active DefaultControlLayout, which grabs the active InputService
            _input = new CharacterInputHandler(new DefaultControlLayout());
        }

        public override void RemoveEventLinks()
        {
            base.RemoveEventLinks();
            EntityLogic.RemoveEventLinks();
            EntityLogic.Body.OnBodyDies -= RaiseCharacterDies;
            EntityLogic.Body.OnBodyDiedCompletely -= RaiseCharacterCompletelyDied;

            _input.RemoveSubscriber(InputType.Movement, CharacterInputHandler.ActionState.Performed, OnMovement);
            _input.RemoveSubscriber(InputType.Movement, CharacterInputHandler.ActionState.Canceled, OnMovement);

            _input.RemoveSubscriber(InputType.FastAttack, CharacterInputHandler.ActionState.Performed, OnFastAttack);
            _input.RemoveSubscriber(InputType.HeavyAttack, CharacterInputHandler.ActionState.Performed, OnHeavyAttack);

            if (CharacterLogic is IUpgradableCharacterLogic upgradableCharacterLogic)
            {
                upgradableCharacterLogic.OnIntroUpgradesReceived -= RaiseCharacterFinishesIntro;
            }
        }

        public override void Dispose()
        {
            // turn off the inputs when the player is destroyed
            _input?.Disable();
            base.Dispose();
        }

        #endregion

        private void OnMovement(InputAction.CallbackContext context)
        {
            if (CharacterLogic.Body.Movement == null)
            {
                return;
            }

            Vector2 direction = context.ReadValue<Vector2>();

            if (direction.sqrMagnitude > 0)
            {
                CharacterLogic.Body.Movement.MoveAsync(direction).Forget();
            }
            else
            {
                CharacterLogic.Body.Movement.Stop();
            }
        }

        public void SetCanAttackFlag(bool value)
        {
            _canAttack = value;
        }

        private void OnFastAttack(InputAction.CallbackContext context)
        {
            if (!_canAttack)
            {
                return;
            }

            CharacterLogic.PerformBasicAttack(BasicAttack.LocalType.Fast);
        }

        private void OnHeavyAttack(InputAction.CallbackContext context)
        {
            if (!_canAttack) return;

            CharacterLogic.PerformBasicAttack(BasicAttack.LocalType.Heavy);
        }

        private void RaiseCharacterFinishesIntro(IUpgradableCharacterLogic characterLogic)
        {
            OnCharacterFinishesIntro?.Invoke(this);
        }

        private void RaiseCharacterDies()
        {
            CharacterLogic.Body.Physics.SetStatic();
            OnCharacterDies?.Invoke(this);
        }

        private void RaiseCharacterCompletelyDied()
        {
            OnCharacterDeathEnd?.Invoke(this);
        }

        #endregion
    }
}
