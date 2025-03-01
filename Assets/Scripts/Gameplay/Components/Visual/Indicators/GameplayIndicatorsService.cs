using Gameplay.Components;
using Settings;
using Settings.Global;
using UnityEngine;
using Zenject;

namespace Gameplay.Visual
{
    public class GameplayIndicatorsService : IService
    {
        private IndicatorPoolData _poolData;
        private IndicatorsPool _indicatorsPool;
        private IndicatorServiceData _data;
        private GameObjectsContainer _container;


        [Inject]
        public GameplayIndicatorsService(IndicatorPoolData poolData, IndicatorServiceData data, GameObjectsContainer container)
        {
            _poolData = poolData;
            _container = container;
            _data = data;

            _indicatorsPool = new IndicatorsPool(
                _poolData,
                data.TextIndicator.gameObject,
                _container.GetChild(data.TextIndicator.name, true),
                _poolData.PreloadInstancesCount
                );
        }

        public void DisplayIndicator(string text, Vector3 position, Color color)
        {
            GameObject indicatorObj = _indicatorsPool.RequestObjectForce();

            if (indicatorObj == null)
            {
                return;
            }

            ITextIndicator textIndicator = indicatorObj.GetComponent<ITextIndicator>();

            textIndicator.TMPText.text = text;
            textIndicator.TMPText.color = color;
            textIndicator.OnLifeTimeEnd += HideIndicatorListener;

            indicatorObj.transform.position = position
                + new Vector3(_data.Offset.x, _data.Offset.y, 0f)
                + new Vector3(Random.Range(-_data.RandomOffset.x, _data.RandomOffset.x), Random.Range(0, _data.RandomOffset.y), 0);

            indicatorObj.SetActive(true);
        }

        public void HideIndicator(GameObject indicatorObj)
        {
            _indicatorsPool.ReturnObject(indicatorObj);
        }

        private void HideIndicatorListener(ITextIndicator sender)
        {
            sender.OnLifeTimeEnd -= HideIndicatorListener;
            MonoBehaviour indicatorMono = sender as MonoBehaviour;
            HideIndicator(indicatorMono.gameObject);
        }
    }
}
