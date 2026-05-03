using UnityEngine;

namespace Characters.Common.Combat
{
    [RequireComponent(typeof(AttackTrigger))]
    public class AttackTriggerVisual : MonoBehaviour
    {
        [Tooltip("Drag the GameObject containing SpriteRenderer or ParticleSystem here")]
        [SerializeField] private GameObject _visualRoot;

        private AttackTrigger _trigger;

        protected virtual void Awake()
        {
            _trigger = GetComponent<AttackTrigger>();
            _trigger.OnTriggerActivation += ShowVisual;
            _trigger.OnTriggerDeactivation += HideVisual;
            HideVisual(null);
        }

        private void OnDestroy()
        {
            if (_trigger != null)
            {
                _trigger.OnTriggerActivation -= ShowVisual;
                _trigger.OnTriggerDeactivation -= HideVisual;
            }
        }

        protected virtual void ShowVisual(IAttackTrigger trigger)
        {
            if (_visualRoot != null) _visualRoot.SetActive(true);
        }

        protected virtual void HideVisual(IAttackTrigger trigger)
        {
            if (_visualRoot != null) _visualRoot.SetActive(false);
        }
    }
}
