using Characters.Common;
using Characters.Common.Movement;
using Characters.Health;
using Characters.Interfaces;
using Characters.Player.Animation;
using System;

namespace Characters.Player
{
    public class PlayerCharacterBody : EntityBodyBase, IDamageable
    {
        #region Fields

        private ICharacterLogic _playerLogic;
        private CharacterAnimatorController _animatorController;

        #endregion

        #region Properties

        #endregion


        #region Methods 

        #region Init

        protected override void InitComponents()
        {
            _playerLogic = GetComponent<ICharacterLogic>();
            Visual = GetComponentInChildren<PlayerCharacterVisual>();

            Health = new EntityHealth(_playerLogic.Stats.Health);
            Invincibility = new CharacterInvincibility(Visual.Renderer, _playerLogic.Stats.InvincibilityTime, _playerLogic.Stats.InvincibilityColor);
        }

        protected override void InitView()
        {
            View = new CharacterLookDirection(transform);
        }

        protected override void InitMovement()
        {
            Movement = new PlayerCharacterMovement(this);
            Movement.UpdateSpeedSettings(new SpeedSettings() { MaxSpeedMultiplier = _playerLogic.Stats.Speed }, true);
        }

        protected override void PostInit()
        {
            _animatorController = Visual.GetAnimatorController<CharacterAnimatorController>();
        }

        public override void ConfigureEventLinks()
        {
            base.ConfigureEventLinks();

            Movement.Speed.OnActualSpeedChanged += _animatorController.SpeedUpdateListener;
            OnBodyDamaged += Invincibility.Enable;

            OnBodyDies += _animatorController.TriggerDeath;
            OnBodyDies += Movement.Block;

        }

        public override void RemoveEventLinks()
        {
            base.RemoveEventLinks();

            Movement.Speed.OnActualSpeedChanged -= _animatorController.SpeedUpdateListener;
            OnBodyDamaged -= Invincibility.Enable;

            OnBodyDies -= _animatorController.TriggerDeath;
            OnBodyDies -= Movement.Block;

            _animatorController.Events.OnDeathFinished -= RaiseOnBodyCompletelyDied;
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
                RaiseOnBodyDies();
                _animatorController.Events.OnDeathFinished += RaiseOnBodyCompletelyDied;
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
