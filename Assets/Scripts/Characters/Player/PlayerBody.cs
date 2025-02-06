using Characters.Health;
using Characters.Interfaces;
using Characters.Player.Animation;
using System;
using UnityEngine;

namespace Characters.Player
{
    public class PlayerBody : CharacterBodyBase, IDamageable
    {
        #region Fields

        private ICharacterLogic _playerLogic;

        #endregion


        #region Properties

        public bool IsDead => (Health != null) && Health.IsEmpty;

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

        protected override void InitReferences()
        {
            try
            {
                CharacterAnimatorController animatorController = Visual.GetAnimatorController<CharacterAnimatorController>();

                Movement.Speed.OnActualSpeedChanged += animatorController.SpeedUpdateListener;
                OnBodyDamaged += Invincibility.Enable;

                OnBodyDeath += animatorController.TriggerDeath;
                OnBodyDeath += Movement.Block;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }

        public override void Dispose()
        {
            CharacterAnimatorController animatorController = Visual.GetAnimatorController<CharacterAnimatorController>();

            Movement.Speed.OnActualSpeedChanged -= animatorController.SpeedUpdateListener;
            OnBodyDamaged -= Invincibility.Enable;
            OnBodyDeath -= animatorController.TriggerDeath;
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
            if(Invincibility.IsActive || IsDead)
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
            if(IsDead)
            {
                return;
            }

            Health.Heal(amount);
        }

        #endregion
    }
}