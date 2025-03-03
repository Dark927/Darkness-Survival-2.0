
using System;
using Characters.Common.Combat;
using Characters.Common.Movement;
using Characters.Common.Physics2D;
using Characters.Common.Visual;
using Characters.Health;
using Characters.Interfaces;
using UnityEngine;
using Utilities.Characters;

namespace Characters.Common
{
    [RequireComponent(typeof(IEntityPhysics2D))]
    public abstract class EntityPhysicsBodyBase : MonoBehaviour, IEntityPhysicsBody, IDamageable
    {
        #region Fields

        private IEntityPhysics2D _entityPhysics;
        private IEntityMovement _movement;
        private IEntityView _view;
        private IEntityVisual _visual;
        private IHealth _characterHealth;
        private IInvincibility _characterInvincibility;
        private bool _isReady = false;

        #endregion


        #region Events

        public event Action OnBodyDies;
        public event Action OnBodyDiedCompletely;
        public event DamageEventHandler OnBodyDamaged;
        public event DamageEventHandlerWithArgs OnBodyDamagedWithArgs;

        #endregion


        #region Properties

        public Transform Transform => transform;
        public IEntityPhysics2D Physics => _entityPhysics;
        public IEntityMovement Movement { get => _movement; protected set => _movement = value; }
        public IEntityView View { get => _view; protected set => _view = value; }
        public IEntityVisual Visual { get => _visual; protected set => _visual = value; }
        public IHealth Health { get => _characterHealth; protected set => _characterHealth = value; }
        public IInvincibility Invincibility { get => _characterInvincibility; protected set => _characterInvincibility = value; }
        public bool IsReady => _isReady;
        public bool IsDying => (Health != null) && Health.IsEmpty;
        public bool CanAcceptDamage => !Invincibility.IsActive && !IsDying;


        #endregion


        #region Methods 

        #region Init

        public virtual void Initialize()
        {
            InitComponents();
            InitMovement();
            Visual.Initialize();
            InitView();
            PostInit();
            _isReady = true;

#if UNITY_EDITOR
            CharacterSettingsValidator.CheckCharacterBodyStatus(this);
#endif
        }

        protected virtual void Start()
        {

        }

        protected virtual void InitComponents()
        {
            _entityPhysics = GetComponent<IEntityPhysics2D>();
            _entityPhysics.Initialize();
        }

        protected virtual void InitMovement()
        {
            _movement = new CharacterStaticMovement();
        }

        protected virtual void InitView()
        {

        }

        protected virtual void PostInit()
        {

        }

        public virtual void ConfigureEventLinks()
        {
            Movement?.ConfigureEventLinks();
        }

        public virtual void RemoveEventLinks()
        {
            Movement?.RemoveEventLinks();
        }

        public virtual void Dispose()
        {
            Visual?.Dispose();
        }

        protected void OnDestroy()
        {
            Dispose();
        }

        #endregion

        public virtual void ResetState()
        {
            Health.ResetState();
        }
        public virtual void TakeDamage(Damage damage)
        {
            if (!CanAcceptDamage)
            {
                return;
            }

            Health.TakeDamage(damage);
            RaiseOnBodyDamaged(damage);

            if (IsDying)
            {
                RaiseOnBodyDies();
                StartBodyDieActions();
            }
        }

        protected virtual void StartBodyDieActions()
        {

        }

        protected void RaiseOnBodyDies()
        {
            OnBodyDies?.Invoke();
        }

        protected void RaiseOnBodyCompletelyDied()
        {
            OnBodyDiedCompletely?.Invoke();
        }

        protected void RaiseOnBodyDamaged(Damage damage)
        {
            OnBodyDamaged?.Invoke();
            OnBodyDamagedWithArgs?.Invoke(this, damage);
        }


        #endregion
    }
}
