using System;
using Characters.Health;
using Characters.Interfaces;
using Characters.Player.Animation;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerCharacterBody : CharacterBodyBase, IDamageable
    {
        #region Fields

        private ICharacterLogic _playerLogic;
        private CharacterAnimatorController _animatorController;

        #endregion

        #region Properties


        #endregion


        #region Methods 

        #region Init

        protected override void Init()
        {
            _playerLogic = GetComponent<ICharacterLogic>();
            Visual = GetComponentInChildren<PlayerVisual>();
            Health = new CharacterHealth(_playerLogic.Stats.Health);
            Invincibility = new CharacterInvincibility(Visual.Renderer, _playerLogic.Stats.InvincibilityTime, _playerLogic.Stats.InvincibilityColor);
        }

        protected override void InitView()
        {
            View = new CharacterLookDirection(transform);
        }

        protected override void InitMovement()
        {
            Movement = new PlayerMovement(this);
            CharacterSpeed speed = new CharacterSpeed() { MaxSpeedMultiplier = _playerLogic.Stats.Speed };
            Movement.Speed.Set(speed);
        }

        protected override void Start()
        {
            try
            {
                _animatorController = Visual.GetAnimatorController<CharacterAnimatorController>();
                SetReferences();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        protected override void SetReferences()
        {
            Movement.Speed.OnActualSpeedChanged += _animatorController.SpeedUpdateListener;
            OnBodyDamaged += Invincibility.Enable;

            OnBodyDeath += _animatorController.TriggerDeath;
            OnBodyDeath += Movement.Block;
        }


        public override void Dispose()
        {
            Movement.Speed.OnActualSpeedChanged -= _animatorController.SpeedUpdateListener;
            OnBodyDamaged -= Invincibility.Enable;

            OnBodyDeath -= _animatorController.TriggerDeath;
            OnBodyDeath -= Movement.Block;
        }

        #endregion


        private void FixedUpdate()
        {
            MoveForward();
        }

        private void MoveForward()
        {
            Movement?.Move();
            View?.LookForward(Movement.Direction);
        }

        public void TakeDamage(float damage)
        {
            if (Invincibility.IsActive || IsDead)
            {
                return;
            }

            Health.TakeDamage(damage);
            RaiseOnBodyDamaged();

            if (Health.IsEmpty)
            {
                RaiseOnBodyDeath();
            }
        }

        public void Heal(float amount)
        {
            if (IsDead)
            {
                return;
            }

            Health.Heal(amount);
        }

        #endregion
    }
}