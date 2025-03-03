using System.Collections;
using UnityEngine;

namespace Effects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FadeOutEffect : MonoBehaviour
    {
        #region Fields 

        private Coroutine _fadeOutRoutine;
        private SpriteRenderer _spriteRenderer;

        #endregion


        #region Methods

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void FadeOut(float fadeOutTimeInSec)
        {
            Debug.Log("r");
            if ((_fadeOutRoutine != null)
                || (_spriteRenderer == null)
                || (_spriteRenderer.color.a == 0))
            {
                return;
            }

            _fadeOutRoutine = StartCoroutine(FadeOutRoutine(fadeOutTimeInSec));
        }

        private IEnumerator FadeOutRoutine(float fadeOutTimeInSec)
        {
            float elapsedTime = 0f;
            float startOpacity = _spriteRenderer.color.a;
            Color tempColor = new Color();

            while (elapsedTime < fadeOutTimeInSec)
            {
                elapsedTime += Time.deltaTime;
                tempColor.a = Mathf.Lerp(startOpacity, 0, elapsedTime / fadeOutTimeInSec);
                _spriteRenderer.color = tempColor;

                yield return null;
            }

            tempColor.a = 0;
            _spriteRenderer.color = tempColor;

            _fadeOutRoutine = null;
        }

        #endregion
    }
}
