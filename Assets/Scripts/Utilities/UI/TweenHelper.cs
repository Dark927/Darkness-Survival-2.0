
using System;
using DG.Tweening;

namespace Utilities.UI
{
    public class TweenHelper
    {
        /// <summary>
        /// Create tween if target object is not null
        /// </summary>
        /// <param name="targetObj">target object</param>
        /// <param name="tweenCreationLogic">the creation logic for the tween</param>
        /// <param name="createdTween">out the created tween if targetObj is correct, else - null</param>
        /// <returns>true if tween was created successfully, else - false</returns>
        public static bool TryCreateTween(UnityEngine.Object targetObj, Func<Tween> tweenCreationLogic, out Tween createdTween)
        {
            createdTween = null;

            if (targetObj != null)
            {
                createdTween = tweenCreationLogic()
                         .Pause();
                return true;
            }

            return false;
        }

        public static void KillTweenIfActive(Tween animation, bool complete = false)
        {
            if (animation != null && animation.active)
            {
                animation.Kill(complete);
            }
        }
    }
}
