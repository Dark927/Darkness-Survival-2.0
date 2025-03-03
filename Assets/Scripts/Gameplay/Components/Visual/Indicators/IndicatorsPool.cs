using Gameplay.Components;
using Settings;
using UnityEngine;

namespace Gameplay.Visual
{
    public class IndicatorsPool : ObjectPoolBase<GameObject>
    {
        #region Methods

        #region Init

        public IndicatorsPool(IndicatorPoolData poolSettings, GameObject indicatorPrefab, int preloadCount = ObjectPoolData.NotIdentifiedPreloadCount) :
            base(poolSettings, indicatorPrefab)
        {
            InitPool(preloadCount);
        }

        public IndicatorsPool(IndicatorPoolData poolSettings, GameObject indicatorPrefab, GameObjectsContainer container, int preloadCount = ObjectPoolData.NotIdentifiedPreloadCount) :
            base(poolSettings, indicatorPrefab, container)
        {
            InitPool(preloadCount);
        }

        #endregion

        protected override GameObject PreloadFunc(GameObject prefab, GameObjectsContainer container = null)
        {
            GameObject createdObj = base.PreloadFunc(prefab, container);
            ITextIndicator indicator = createdObj.GetComponent<TextIndicator>();
            indicator.Initialize();

            return createdObj;
        }

        protected override void ReturnAction(GameObject obj)
        {
            base.ReturnAction(obj);

            obj.SetActive(false);
        }

        #endregion
    }

}
