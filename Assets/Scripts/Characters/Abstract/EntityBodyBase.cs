
using Characters.Common.Movement;
using Characters.Common.Visual;
using Characters.Health;
using Characters.Interfaces;
using System;
using UnityEngine;
using Utilities.Characters;

namespace Characters.Common
{
    public abstract class EntityBodyBase : MonoBehaviour, IEntityBody
    {
        #region Fields

        private CharacterMovementBase _movement;
        private ICharacterView _view;
        private EntityVisualBase _visual;
        private IHealth _characterHealth;
        private IInvincibility _characterInvincibility;
        private bool _isReady = false;

        #endregion


        #region Events

        public event Action OnBodyDies;
        public event Action OnBodyDiedCompletely;
        public event Action OnBodyDamaged;

        #endregion


        #region Properties

        public Transform Transform => transform;
        public CharacterMovementBase Movement { get => _movement; protected set => _movement = value; }
        public ICharacterView View { get => _view; protected set => _view = value; }
        public EntityVisualBase Visual { get => _visual; protected set => _visual = value; }
        public IHealth Health { get => _characterHealth; protected set => _characterHealth = value; }
        public IInvincibility Invincibility { get => _characterInvincibility; protected set => _characterInvincibility = value; }
        public bool IsReady => _isReady;
        public bool IsDead => (Health != null) && Health.IsEmpty;


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

        protected abstract void InitComponents();

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

        protected void RaiseOnBodyDies()
        {
            OnBodyDies?.Invoke();
        }

        protected void RaiseOnBodyCompletelyDied()
        {
            OnBodyDiedCompletely?.Invoke();
        }

        protected void RaiseOnBodyDamaged()
        {
            OnBodyDamaged?.Invoke();
        }

        #endregion
    }
}
