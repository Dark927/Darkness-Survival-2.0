using Characters.Health.HealthBar;
using Characters.Player.Controls;
using Characters.Player.Data;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Characters.Player
{
    public class Player : MonoBehaviour, IDisposable
    {
        private PlayerInput _input;
        private ICharacterLogic _character;
        private IHealthBar _healthBar;

        public PlayerInput Input => _input;

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

            _character.Body.OnBodyDeath += CharacterDeathListener;
        }

        private void CharacterDeathListener(object sender, EventArgs args)
        {
            _healthBar.Hide();
            _input.Disable();

            if(_character is MonoBehaviour _characterComponent)
            {
                Rigidbody2D _rigidbody = _characterComponent.GetComponent<Rigidbody2D>();
                _rigidbody.isKinematic = true;
            }
        }

        private void InitInput()
        {
            IControlLayout controlLayout = new DefaultControlLayout();
            InputHandler inputHandler = new InputHandler(controlLayout);
            _input = new PlayerInput(inputHandler);
        }

        public void Dispose()
        {

        }
    }
}