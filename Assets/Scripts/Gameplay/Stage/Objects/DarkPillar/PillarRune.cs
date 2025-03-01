using UnityEngine;

namespace Gameplay.Stage.Objects
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class PillarRune : PillarPart
    {
        #region Fields 

        [SerializeField] private Sprite _runeSprite;
        private SpriteRenderer _spriteRenderer;

        #endregion


        #region Properties

        #endregion


        #region Methods

        #region Init

        public override void Initialize()
        {
            base.Initialize();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = _runeSprite;
        }

        #endregion

        #endregion
    }
}
