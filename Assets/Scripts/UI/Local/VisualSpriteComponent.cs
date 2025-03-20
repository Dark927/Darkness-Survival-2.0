using UnityEngine;
using Utilities.ErrorHandling;

namespace UI.Local
{
    [ExecuteAlways]
    public abstract class VisualSpriteComponent : MonoBehaviour
    {
        #region Fields

        private SpriteRenderer _renderer;

        #endregion


        #region Properties

        public SpriteRenderer Renderer => _renderer;

        #endregion


        #region Methods 

        #region Init

        public void Initialize()
        {
            _renderer = TryGetSpriteRenderer();

            if (_renderer == null)
            {
                ErrorLogger.LogComponentIsNull(gameObject.name, nameof(SpriteRenderer));
                gameObject.SetActive(false);
            }
        }

        private SpriteRenderer TryGetSpriteRenderer()
        {
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();

            if (renderer == null)
            {
                renderer = GetComponentInChildren<SpriteRenderer>();
            }

            return renderer;
        }

        #endregion

        public void Hide()
        {
            Renderer.enabled = false;
        }

        public void Show()
        {
            Renderer.enabled = true;
        }

        #endregion

    }
}
