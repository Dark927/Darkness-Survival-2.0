
using System;
using Characters.Common.Combat;
using Characters.Common.Movement;
using Characters.Common.CustomPhysics2D;
using Characters.Common.Visual;
using Characters.Health;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Utilities.Characters;

namespace Characters.Common
{
    [RequireComponent(typeof(IEntityPhysics2D))]
    public abstract class EntityPhysicsBodyBase : MonoBehaviour, IEntityPhysicsBody
    {
        #region Fields

        [Header("Targeting")]
        [SerializeField] private Transform _targetingTransform;
        [Tooltip("If true and Targeting Transform is null, automatically creates a targeting point at the Collider's local center.")]
        [SerializeField] private bool _useColliderCenterAsTarget = true;
        [SerializeField] private string _customTargetingObjectName = "TargetingCenter";

        private IEntityPhysics2D _entityPhysics;
        private IEntityMovement _movement;
        private IEntityView _view;
        private IEntityVisual _visual;
        private IEntityLight _entityLight;

        private IHealth _characterHealth;
        private IInvincibility _characterInvincibility;

        private DamageableType _damageableType;
        private bool _isReady = false;

        #endregion


        #region Events

        public event Action OnBodyDies;
        public event Action<IAttackable> OnKilled;
        public event Action OnBodyDiedCompletely;
        public event DamageEventHandler OnBodyDamaged;
        public event DamageEventHandlerWithArgs OnBodyDamagedWithArgs;
        public event Action<IEntityPhysicsBody> OnBodyDiesWithArgs;
        public event Action<IEntityPhysicsBody> OnBodyDiedCompletelyWithArgs;

        #endregion


        #region Properties

        public Transform TargetingTransform => _targetingTransform;
        public Transform OriginalTransform => transform;
        public IEntityPhysics2D Physics => _entityPhysics;
        public IEntityMovement Movement { get => _movement; protected set => _movement = value; }
        public IEntityView View { get => _view; protected set => _view = value; }
        public IEntityVisual Visual { get => _visual; protected set => _visual = value; }
        public IHealth Health { get => _characterHealth; protected set => _characterHealth = value; }
        public IInvincibility Invincibility { get => _characterInvincibility; protected set => _characterInvincibility = value; }
        public bool IsReady => _isReady;
        public bool IsDying => (Health != null) && Health.IsEmpty;
        public bool CanAcceptDamage => !Invincibility.IsActive && !IsDying;
        public DamageableType Type => _damageableType;


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
            InitLight();
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

            if (_targetingTransform == null)
            {
                // Attempt to auto-generate the center target if allowed and a collider exists
                if (_useColliderCenterAsTarget && Physics.Collider != null)
                {
                    GameObject targetObj = new GameObject(_customTargetingObjectName);
                    _targetingTransform = targetObj.transform;

                    _targetingTransform.SetParent(transform);

                    // The collider's offset is in local space, making it the perfect relative position
                    _targetingTransform.localPosition = Physics.Collider.offset;
                }
                else
                {
                    // Fallback to the entity's root pivot point
                    _targetingTransform = OriginalTransform;
                }
            }
        }

        protected virtual void InitMovement()
        {
            _movement = new CharacterStaticMovement();
        }

        protected virtual void InitView()
        {

        }

        protected virtual void InitLight()
        {
            Light2D targetLight = GetComponentInChildren<Light2D>();

            if (targetLight != null)
            {
                _entityLight = new EntityLight(targetLight);
                _entityLight.SetLightIntensityLimit(targetLight.intensity);
            }
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
            Health?.Dispose();
        }

        #endregion

        public virtual void ResetState()
        {
            Health.ResetState();
        }
        public virtual void TakeDamage(IAttackable sender, Damage damage)
        {
            if (!CanAcceptDamage)
            {
                return;
            }

            Health.ReduceCurrentHp(damage.Amount);
            RaiseOnBodyDamaged(damage);

            if (IsDying)
            {
                OnKilled?.Invoke(sender);
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
            OnBodyDiesWithArgs?.Invoke(this);
        }

        protected void RaiseOnBodyCompletelyDied()
        {
            OnBodyDiedCompletely?.Invoke();
            OnBodyDiedCompletelyWithArgs?.Invoke(this);
        }

        protected void RaiseOnBodyDamaged(Damage damage)
        {
            OnBodyDamaged?.Invoke();
            OnBodyDamagedWithArgs?.Invoke(this, damage);
        }

        public bool TryGetEntityLight(out IEntityLight light)
        {
            light = _entityLight;
            return light != null;
        }

        public void SetDamageableType(DamageableType type)
        {
            _damageableType = type;
        }


        #endregion
    }
}
