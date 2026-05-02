using Gameplay.Components;
using UnityEngine;

namespace Gameplay.Visual
{
    public class TextIndicatorsPool : ComponentsPoolBase<TextIndicator>
    {
        #region Methods

        #region Init

        public TextIndicatorsPool(ObjectPoolSettings poolSettings, GameObject indicatorPrefab, Transform container) :
            base(poolSettings, indicatorPrefab, container)
        {

        }

        #endregion

        protected override TextIndicator PreloadFunc(Transform container = null)
        {
            var createdIndicator = base.PreloadFunc(container);
            createdIndicator.Initialize();

            return createdIndicator;
        }

        protected override void ReturnAction(TextIndicator obj)
        {
            base.ReturnAction(obj);

            obj.gameObject.SetActive(false);
        }

        #endregion
    }

}
