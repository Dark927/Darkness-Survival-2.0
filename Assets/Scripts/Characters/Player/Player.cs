using Characters.Health.HealthBar;
using Characters.Player.Controls;
using Settings.Global;
using System;
using UnityEngine;

namespace Characters.Player
{
    public class Player : MonoBehaviour, IDisposable
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


        #region Methods

        #region Init 

        private void Awake()
        {
            InitInput();

            _character = GetComponentInChildren<ICharacterLogic>();
            _healthBar = GetComponentInChildren<IHealthBar>();
        }

        private void Start()
        {
            ServiceLocator.Current.Get<PlayerManager>()?.AddPlayer(this);

            _input.SetMovement(_character.Body.Movement);
            _input.SetBasicAttacks(_character.BasicAttacks);

            _character.Body.OnBodyDeath += PlayerCharacterDie;
        }

        private void InitInput()
        {
            IControlLayout controlLayout = new DefaultControlLayout();
            InputHandler inputHandler = new InputHandler(controlLayout);
            _input = new PlayerInput(inputHandler);
        }

        public void Dispose()
        {
            _character.Body.OnBodyDeath -= PlayerCharacterDie;
            _input?.Dispose();
        }

        #endregion

        private void PlayerCharacterDie()
        {
            _healthBar.Hide();
            _input.Disable();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion
    }
}