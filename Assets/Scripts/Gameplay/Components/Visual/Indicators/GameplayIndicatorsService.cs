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
        private TextIndicatorsPool _indicatorsPool;
        private IndicatorServiceData _data;
        private GameObjectsContainer _container;


        [Inject]
        public GameplayIndicatorsService(IndicatorPoolData poolData, IndicatorServiceData data, GameObjectsContainer container)
        {
            _poolData = poolData;
            _container = container;
            _data = data;

            _indicatorsPool = new TextIndicatorsPool(
                _poolData.Settings,
                data.TextIndicator.gameObject,
                _container.GetChild(data.TextIndicator.name, true).transform
                );

            _indicatorsPool.Initialize();
        }

        // ToDo : refactore indicator logic, remove GameObject from ITextIndicator interface 
        public void DisplayIndicator(string text, Vector3 position, Color color)
        {
            ITextIndicator textIndicator;

            if (_poolData.ForceReuseEnabled)
            {
                textIndicator = _indicatorsPool.RequestObjectForce();
            }
            else
            {
                textIndicator = _indicatorsPool.RequestObject();
            }

            if (textIndicator == null)
            {
                return;
            }

            textIndicator.TMPText.text = text;
            textIndicator.TMPText.color = color;
            textIndicator.OnLifeTimeEnd += HideIndicatorListener;

            textIndicator.gameObject.transform.position = position
                + new Vector3(_data.Offset.x, _data.Offset.y, 0f)
                + new Vector3(Random.Range(-_data.RandomOffset.x, _data.RandomOffset.x), Random.Range(0, _data.RandomOffset.y), 0);

            textIndicator.gameObject.SetActive(true);
        }

        public void HideIndicator(ITextIndicator indicator)
        {
            _indicatorsPool.ReturnItem(indicator as TextIndicator); // ToDo : maybe create IndicatorBase and use it instead.
        }

        private void HideIndicatorListener(ITextIndicator sender)
        {
            sender.OnLifeTimeEnd -= HideIndicatorListener;
            HideIndicator(sender);
        }
    }
}
