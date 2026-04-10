using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;

namespace Utilities.Animations
{
    public static class DoTweenExtension
    {
        /// <summary>
        /// Tweens a TextMeshProUGUI font size to the given value.
        /// Also stores the TextMeshProUGUI as the tween's target so it can be used for filtered operations.
        /// </summary>
        /// <param name="endValue">The end font size value to reach</param>
        /// <param name="duration">The duration of the tween</param>
        public static TweenerCore<float, float, FloatOptions> DOScale(this TextMeshProUGUI target, float endValue, float duration)
        {
            // Create a tween to animate the font size of the text
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.fontSize, x => target.fontSize = x, endValue, duration);

            // Set the target to be the TextMeshProUGUI component for future filtered operations
            t.SetTarget(target);
            return t;
        }
    }
}
