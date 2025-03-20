using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities.Attributes;

namespace UI.Characters.Upgrades
{

    /// <summary>
    /// This component is used to hold upgrade cards and configure their layout.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class UpgradeCardsHolderUI : MonoBehaviour
    {
        [CustomHeader("Visual Settings UI", 3, 0)]
        [Tooltip("Use this to provide the min free space which must be available in the container to scale up all items")]
        [SerializeField] private float _minFreeSpaceToScaleUp = 50f;
        [SerializeField] private Vector2 _maxElementSize;
        [SerializeField] private float _extraScaling = 0f;

        private RectTransform _containerRect;
        private List<UpgradeCardUI> _cards;

        public IEnumerable<UpgradeCardUI> Cards => _cards;


        private void Awake()
        {
            _containerRect = GetComponent<RectTransform>();
            _cards = GetComponentsInChildren<UpgradeCardUI>(true).ToList();
            _cards.ForEach(card => card.Initialize());
        }

        public void UpdateLayout()
        {
            ArrangeWithAdaptiveScale();
        }

        private void ArrangeWithAdaptiveScale()
        {
#if UNITY_EDITOR
            _containerRect = GetComponent<RectTransform>();
#endif

            _cards = GetComponentsInChildren<UpgradeCardUI>(true).ToList();

            if (_cards.Count == 0)
            {
                return;
            }

            List<RectTransform> activeCardTransforms = _cards
                .Where(card => card.gameObject.activeInHierarchy)
                .Select((card) => card.GetComponent<RectTransform>()).ToList();

            if(activeCardTransforms.Count == 0)
            {
                return;
            }

            ApplyAdaptiveScale(_containerRect, activeCardTransforms);
            Arrange(activeCardTransforms);
        }

        private void ApplyAdaptiveScale(RectTransform containerRect, List<RectTransform> targetTransforms)
        {
            int elementsCount = targetTransforms.Count;
            var firstElement = targetTransforms.First();

            float containerWidth = containerRect.rect.width;
            float elementsCommonWidth = elementsCount * (firstElement.rect.width * firstElement.localScale.x);
            elementsCommonWidth += (_extraScaling * -1) * (elementsCount - 1); // remove extra spacing

            float widthDiff = Mathf.Abs(elementsCommonWidth - containerWidth);
            float diffPerOne = (widthDiff / (firstElement.localScale.x * elementsCount));
            Vector2 commonSize = firstElement.rect.size;

            // Scale down

            if (elementsCommonWidth > (containerWidth + 0.01f)
                || (_maxElementSize.x < firstElement.rect.width)
                || (_maxElementSize.y < firstElement.rect.height))
            {
                Vector2 sizeDelta = new Vector2(
                    Mathf.Clamp(commonSize.x - diffPerOne, 0, _maxElementSize.x),
                    Mathf.Clamp(commonSize.y - diffPerOne, 0, _maxElementSize.y)
                );

                Scale(targetTransforms, sizeDelta);
                return;
            }


            // Scale up

            if (widthDiff > _minFreeSpaceToScaleUp)
            {
                Vector2 sizeDelta = new Vector2(
                    Mathf.Clamp(commonSize.x + diffPerOne, 0, _maxElementSize.x),
                    Mathf.Clamp(commonSize.y + diffPerOne, 0, _maxElementSize.y)
                );

                Scale(targetTransforms, sizeDelta);
                return;
            }
        }

        private void Scale(List<RectTransform> targetTransforms, Vector2 targetSizeDelta)
        {
            foreach (var currentTransform in targetTransforms)
            {
                currentTransform.sizeDelta = targetSizeDelta;
            }
        }

        private void Arrange(List<RectTransform> targetTransforms)
        {
            float containerWidth = _containerRect.rect.width;
            float containerCenterX = containerWidth / 2;

            int elementsCount = targetTransforms.Count;
            int partitionIndex = 0;
            float partitionWidth = containerWidth / elementsCount;

            foreach (var currentTransform in targetTransforms)
            {
                float currentScaledWidth = currentTransform.rect.width * currentTransform.localScale.x;
                float posX = containerCenterX - (partitionWidth * (elementsCount - partitionIndex));

                posX += (partitionWidth / 2f);

                currentTransform.anchoredPosition = new Vector2(posX, 0f);
                partitionIndex += 1;
            }
        }

#if UNITY_EDITOR
        [ExecuteAlways]
        private void Update()
        {
            if(Application.isPlaying)
            {
                return;
            }

            ArrangeWithAdaptiveScale();
        }
#endif
    }
}
