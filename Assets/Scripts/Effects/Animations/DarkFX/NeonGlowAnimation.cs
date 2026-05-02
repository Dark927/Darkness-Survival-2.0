using UnityEngine;
using DG.Tweening;
using Utilities.UI;
using Materials.DarkEntityFX;

namespace Visuals.Effects.Animations
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(DarkEntityFXComponent))]
    public class NeonGlowAnimation : MonoBehaviour
    {
        #region Fields 

        [Header("Glow Intensity")]
        [Tooltip("The maximum brightness the emission will reach.")]
        [SerializeField] private float _maxEmissionAmount = 4f;

        [Tooltip("The minimum brightness the emission will fall to.")]
        [SerializeField] private float _minEmissionAmount = 0.2f;

        [SerializeField] private float _pulseDuration = 1.2f;
        [SerializeField] private Ease _pulseEase = Ease.InOutSine;

        [Header("Glow Color")]
        [Tooltip("If true, the glow will shift through the gradient over time. If false, it uses a solid color.")]
        [SerializeField] private bool _useColorShift = true;

        [GradientUsage(true)]
        [SerializeField] private Gradient _emissionGradient;
        [SerializeField] private float _colorShiftDuration = 3f;

        private DarkEntityFXComponent _entityFXComponent;
        private ParametricProps _properties;

        private Tween _pulseTween;
        private Tween _colorTween;

        // An internal tracker for the gradient evaluation
        private float _gradientTime = 0f;

        #endregion

        #region Methods

        private void Awake()
        {
            _entityFXComponent = GetComponent<DarkEntityFXComponent>();
        }

        private void Start()
        {
            StartEmissionPulse();

            if (_useColorShift && _emissionGradient != null)
            {
                StartColorShift();
            }
        }

        private void OnDestroy()
        {
            TweenHelper.KillTweenIfActive(_pulseTween);
            TweenHelper.KillTweenIfActive(_colorTween);
        }

        #endregion

        #region Animation Logic

        private void StartEmissionPulse()
        {
            // Set initial state
            SetEmissionAmount(_minEmissionAmount);

            // Create a custom animation that drives the SetEmissionAmount function
            bool isCreated = TweenHelper.TryCreateTween(
                _entityFXComponent,
                () => DOVirtual.Float(_minEmissionAmount, _maxEmissionAmount, _pulseDuration, SetEmissionAmount)
                               .SetLoops(-1, LoopType.Yoyo)
                               .SetEase(_pulseEase),
                out _pulseTween);

            if (isCreated)
            {
                _pulseTween.Play();
            }
        }

        private void StartColorShift()
        {
            _gradientTime = 0f;

            // Animate a float from 0 to 1, evaluate the gradient at that point, and apply the color
            bool isCreated = TweenHelper.TryCreateTween(
                _entityFXComponent,
                () => DOVirtual.Float(0f, 1f, _colorShiftDuration, (t) =>
                {
                    _gradientTime = t;
                    SetEmissionColor(_emissionGradient.Evaluate(t));
                })
                               .SetLoops(-1, LoopType.Yoyo)
                               .SetEase(Ease.Linear), // Linear looks best for color gradients
                out _colorTween);

            if (isCreated)
            {
                _colorTween.Play();
            }
        }

        #endregion

        #region Shader Property Setters

        // These methods act as wrappers to safely update custom shader properties

        private void SetEmissionAmount(float amount)
        {
            _properties = _entityFXComponent.MaterialPropContainer.Properties;
            _properties.EmissionAmount = amount;
            _entityFXComponent.MaterialPropContainer.Properties = _properties;
        }

        private void SetEmissionColor(Color color)
        {
            _properties = _entityFXComponent.MaterialPropContainer.Properties;
            _properties.EmissionColor = color;
            _entityFXComponent.MaterialPropContainer.Properties = _properties;
        }

        #endregion
    }
}
