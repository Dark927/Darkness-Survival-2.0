using Characters.Health.HealthBar;
using Characters.Player.Controls;
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
            _input.SetMovement(_character.Body.Movement);
            _input.SetBasicAttacks(_character.BasicAttacks);

            _character.Body.OnBodyDeath += PlayerLost;
        }

        private void InitInput()
        {
            IControlLayout controlLayout = new DefaultControlLayout();
            InputHandler inputHandler = new InputHandler(controlLayout);
            _input = new PlayerInput(inputHandler);
        }

        public void Dispose()
        {
            _input.Dispose();
        }

        #endregion

        private void PlayerLost()
        {
            _healthBar.Hide();
            _input.Disable();

            if (_character is MonoBehaviour _characterComponent)
            {
                Rigidbody2D _rigidbody = _characterComponent.GetComponent<Rigidbody2D>();
                _rigidbody.isKinematic = true;
                Character.Body.Movement.Stop();
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion
    }
}