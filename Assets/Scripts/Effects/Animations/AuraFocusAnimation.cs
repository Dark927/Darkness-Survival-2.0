using UnityEngine;
using DG.Tweening;
using Utilities.UI;

namespace Visuals.Effects.Animations
{
    public class AuraFocusAnimation : MonoBehaviour
    {
        #region Fields 

        [Header("Spawn Animation")]
        [SerializeField] private float _introDuration = 1.2f;
        [SerializeField] private Vector3 _targetScale = new Vector3(1.5f, 1.5f, 1.5f);
        [SerializeField] private Ease _introEase = Ease.OutBack;

        [Header("Ambient Motion")]
        [Tooltip("Degrees per second. Negative numbers spin clockwise.")]
        [SerializeField] private float _rotationSpeed = -30f;

        private Tween _scaleTween;

        #endregion

        #region Methods

        private void Start()
        {
            // Set scale to 0 instantly so it can "pop" in
            transform.localScale = Vector3.zero;

            bool isCreated = TweenHelper.TryCreateTween(
                gameObject,
                () => transform.DOScale(_targetScale, _introDuration)
                               .SetEase(_introEase),
                out _scaleTween);

            if (isCreated)
            {
                _scaleTween.Play();
            }
        }

        private void Update()
        {
            // Apply a constant, slow ambient rotation
            transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
        }

        private void OnDestroy()
        {
            TweenHelper.KillTweenIfActive(_scaleTween);
        }

        #endregion
    }
}
