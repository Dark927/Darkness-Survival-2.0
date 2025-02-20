using Characters.Common;
using Characters.Common.Movement;
using Characters.Health;
using Characters.Interfaces;
using Characters.Player.Animation;

namespace Characters.Player
{
    public class PlayerCharacterBody : EntityPhysicsBodyBase
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
            base.InitComponents();

            _playerLogic = GetComponent<ICharacterLogic>();
            Visual = GetComponentInChildren<PlayerCharacterVisual>();

            Health = new EntityHealth(_playerLogic.Stats.Health);
            Invincibility = new CharacterInvincibility(Visual.Renderer, _playerLogic.Stats.InvincibilityTime, _playerLogic.Stats.InvincibilityColor);
        }

        protected override void InitView()
        {
            View = new EntityLookDirection(transform);
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
            Movement.OnMovementPerformed += View.LookForward;

            OnBodyDies += _animatorController.TriggerDeath;
            OnBodyDies += Movement.Block;
            OnBodyDies += Health.CancelHpRegeneration;
        }

        public override void RemoveEventLinks()
        {
            base.RemoveEventLinks();

            Movement.Speed.OnActualSpeedChanged -= _animatorController.SpeedUpdateListener;
            OnBodyDamaged -= Invincibility.Enable;
            Movement.OnMovementPerformed -= View.LookForward;

            OnBodyDies -= _animatorController.TriggerDeath;
            OnBodyDies -= Movement.Block;
            OnBodyDies -= Health.CancelHpRegeneration;

            _animatorController.Events.OnDeathFinished -= RaiseOnBodyCompletelyDied;
        }

        #endregion

        protected override void StartBodyDieActions()
        {
            _animatorController.Events.OnDeathFinished += RaiseOnBodyCompletelyDied;
        }

        public void Heal(float amount)
        {
            if (IsDying)
            {
                return;
            }

            Health.Heal(amount);
        }

        #endregion
    }
}
