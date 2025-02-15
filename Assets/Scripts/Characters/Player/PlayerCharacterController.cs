using Characters.Common;
using Characters.Interfaces;
using Characters.Player.Controls;
using Settings.Global;
using System;
using Cysharp.Threading.Tasks;
using Characters.Common.Combat.Weapons;
using System.Diagnostics;

namespace Characters.Player
{
    public class PlayerCharacterController : EntityController
    {
        #region Fields 

        private PlayerInput _input;

        #endregion


        #region Properties

        public PlayerInput Input => _input;
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

            PlayerCharacterLogic playerCharacter = (PlayerCharacterLogic)EntityLogic;

            _input.SetMovement(EntityLogic.Body.Movement);
            Character.OnBasicAttacksReady += ConfigureBasicWeaponInput;
        }

        private void ConfigureBasicWeaponInput(BasicAttack attack)
        {
            Character.OnBasicAttacksReady -= ConfigureBasicWeaponInput;
            _input.SetBasicAttacks(attack);
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
            IControlLayout controlLayout = new DefaultControlLayout();
            InputHandler inputHandler = new InputHandler(controlLayout);
            _input = new PlayerInput(inputHandler);
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
            _input?.Dispose();
        }

        #endregion

        private void PlayerCharacterDies()
        {
            _input.Disable();
        }

        private void NotifyCharacterCompletelyDied()
        {
            OnCharacterDeathEnd?.Invoke();
        }

        #endregion
    }
}
