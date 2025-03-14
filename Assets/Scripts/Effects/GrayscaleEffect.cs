using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using System;
using Utilities.UI;
using Settings.Global;

public class GrayscaleEffect : IDisposable
{
    #region Fields 

    private Volume _volume;
    private ColorAdjustments _colorAdjustments;
    private GrayscaleSettings _startSettings;

    private GrayscaleSettings _defaultTargetSettings;
    private float _defaultTransitionDuration;
    private Sequence _currentAnimation;

    #endregion


    #region Methods

    #region Init

    public GrayscaleEffect(Volume volume, GrayscaleSettings defaultTargetSettings = default, float defaultTransitionDurationInSec = 1f)
    {
        _volume = volume;
        _defaultTargetSettings = defaultTargetSettings;
        _defaultTransitionDuration = defaultTransitionDurationInSec;

        if (_volume.profile.TryGet(out _colorAdjustments))
        {
            _colorAdjustments.saturation.overrideState = true;
            _startSettings.Saturation = _colorAdjustments.saturation.value;
            _startSettings.Contrast = _colorAdjustments.contrast.value;
        }

        _defaultTargetSettings = defaultTargetSettings;
    }
    public void Dispose()
    {
        TweenHelper.KillTweenIfActive(_currentAnimation);
    }

    #endregion

    /// <summary>
    /// Apply grayscale effect to the current volume using the colorAdjustments (with default target settings)
    /// </summary>
    public void ApplyGrayscale()
    {
        TweenHelper.KillTweenIfActive(_currentAnimation);
        ApplyGrayscale(_defaultTargetSettings.Saturation, _defaultTransitionDuration, _defaultTargetSettings.Contrast);
    }

    /// <summary>
    /// Apply grayscale effect to the current volume using the colorAdjustments (with custom target settings)
    /// </summary>
    /// <param name="targetIntensity">target grayscale intensity from 0 to 100</param>
    /// <param name="durationInSec">grayscale transition duration</param>
    /// <param name="targetContrast">grayscale contrast intensity from 0 to 100</param>
    public void ApplyGrayscale(float targetIntensity, float durationInSec, float targetContrast = 0)
    {
        if ((targetIntensity < 0 || targetIntensity > 100) ||
            (targetContrast < 0 || targetContrast > 100))
        {
            return;
        }

        TweenHelper.KillTweenIfActive(_currentAnimation);
        _currentAnimation = ApplyGrayscaleEffect(targetIntensity, durationInSec, targetContrast);
    }

    public void RemoveGrayscale()
    {
        TweenHelper.KillTweenIfActive(_currentAnimation);
        RemoveGrayscale(_defaultTransitionDuration);
    }

    public void RemoveGrayscale(float durationInSec)
    {
        TweenHelper.KillTweenIfActive(_currentAnimation);
        _currentAnimation = ApplyGrayscaleEffect(_startSettings.Saturation, durationInSec, _startSettings.Contrast);
    }

    public void SetTransitionDuration(float durationInSec)
    {
        _defaultTransitionDuration = durationInSec;
    }

    private Sequence ApplyGrayscaleEffect(float targetIntensity, float duration, float targetContrast = 0)
    {
        Sequence animation = DOTween.Sequence();

        if (_colorAdjustments == null)
        {
            return animation;
        }

        animation
            .Append(
                DOTween.To(() => _colorAdjustments.saturation.value,
                    x => _colorAdjustments.saturation.value = x, -targetIntensity, duration)
                        .SetEase(Ease.InOutCubic)
        )
            .Join(
                DOTween.To(() => _colorAdjustments.contrast.value,
                    x => _colorAdjustments.contrast.value = x, targetContrast, duration)
                        .SetEase(Ease.InOutCubic)
        );

        return animation;
    }

    #endregion
}
