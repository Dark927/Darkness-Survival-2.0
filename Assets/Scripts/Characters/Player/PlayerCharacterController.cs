using Characters.Common;
using Characters.Interfaces;
using Characters.Player.Controls;
using Settings.Global;
using System;
using UI.Local.Health;

namespace Characters.Player
{
    public class PlayerCharacterController : EntityController
    {
        #region Fields 

        private PlayerInput _input;
        private ICharacterLogic _character;
        private IHealthBar _healthBar;

        #endregion


        #region Properties

        public PlayerInput Input => _input;
        public ICharacterLogic Character => _character;

        #endregion


        #region Events

        public event Action OnCharacterDeathEnd;

        #endregion


        #region Methods

        #region Init 

        private void Awake()
        {
            InitInput();

            _character = GetComponentInChildren<ICharacterLogic>();
            _healthBar = GetComponentInChildren<IHealthBar>();
        }

        protected override void Start()
        {
            base.Start();
            ServiceLocator.Current.Get<PlayerManager>()?.AddPlayer(this);

            ConfigureCharacter();
            ConfigureEventLinks();
            _healthBar.Initialize(Character.Body);
        }

        private void ConfigureCharacter()
        {
            _character.Initialize();
            _character.ConfigureEventLinks();

            PlayerCharacterLogic playerCharacter = (PlayerCharacterLogic)_character;

            _input.SetMovement(_character.Body.Movement);
            _input.SetBasicAttacks(playerCharacter.BasicAttacks);
        }

        public override void ConfigureEventLinks()
        {
            base.ConfigureEventLinks();

            PlayerCharacterVisual visual = (Character.Body.Visual as PlayerCharacterVisual);

            _character.ConfigureEventLinks();
            _character.Body.OnBodyDies += PlayerCharacterDies;
            _character.Body.OnBodyDied += NotifyCharacterCompletelyDied;
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
            _character.RemoveEventLinks();
            _character.Body.OnBodyDies -= PlayerCharacterDies;
            _character.Body.OnBodyDied -= NotifyCharacterCompletelyDied;
        }

        public override void Dispose()
        {
            base.Dispose();
            _input?.Dispose();
        }

        #endregion

        private void PlayerCharacterDies()
        {
            _healthBar.Hide();
            _input.Disable();
        }

        private void NotifyCharacterCompletelyDied()
        {
            OnCharacterDeathEnd?.Invoke();
        }

        #endregion
    }
}
