
using Settings.Global;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class NeroSwordVisual : MonoBehaviour, IInitializable
    {
        #region Fields 

        private SpriteRenderer _spriteRenderer;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        public void Initialize()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            Deactivate();
        }

        #endregion

        public void Activate()
        {
            _spriteRenderer.enabled = true;
        }

        public void Deactivate()
        {
            _spriteRenderer.enabled = false;

        }


        #endregion
    }
}
