
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters.Common.Physics2D
{
    [RequireComponent(typeof(Rigidbody2D), typeof(IEntityDynamicLogic))]
    public class EntityPhysics : MonoBehaviour, IEntityPhysics2D
    {
        #region Fields 

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;

        private bool _isImmune = false;
        private IEntityDynamicLogic _entityLogic;

        private UniTask _activeImmuneTask;
        private CancellationTokenSource _immuneCts;

        [SerializeField] private int _immunityTimeMs = 0;


        #endregion


        #region Properties

        public int ImmunityTimeMs => _immunityTimeMs;
        public bool IsImmune => _isImmune;
        public Rigidbody2D Rigidbody2D => _rigidbody;
        public Collider2D Collider => _collider;
        public IEntityDynamicLogic EntityLogic => _entityLogic;

        #endregion


        #region Methods

        #region Init

        public void Initialize()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _entityLogic = GetComponent<IEntityDynamicLogic>();
        }

        #endregion

        public void TryPerformPhysicsActions(EntityPhysicsActions physicsActions)
        {
            if (_isImmune)
            {
                return;
            }

            foreach (var physicsAction in physicsActions.Get())
            {
                physicsAction.Perform(this);
            }

            _immuneCts ??= new CancellationTokenSource();
            _activeImmuneTask = ActivateImmune(ImmunityTimeMs, _immuneCts.Token);
        }

        private async UniTask ActivateImmune(int timeMs, CancellationToken token = default)
        {
            _isImmune = true;
            await UniTask.Delay(timeMs, cancellationToken: token);
            _isImmune = false;
        }

        public EntityPhysics DisableImmune()
        {
            if (_immuneCts == null)
            {
                _immuneCts.Cancel();
                _immuneCts.Dispose();
                _immuneCts = null;
            }

            return this;
        }

        public void SetStatic()
        {
            _rigidbody.isKinematic = true;
        }

        public void SetDynamic()
        {
            _rigidbody.isKinematic = false;
        }

        #endregion
    }
}
