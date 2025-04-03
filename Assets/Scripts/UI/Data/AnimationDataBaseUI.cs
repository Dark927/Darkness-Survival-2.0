
using Settings;
using UnityEngine;
using Utilities.Attributes;

namespace UI
{
    public abstract class AnimationDataBaseUI<TAnimationParametersUI> : DescriptionBaseData
        where TAnimationParametersUI : IAnimationParamsUI, new()
    {
        #region Fields 

        [CustomHeader("Hover", count: 2, depth: 0, headerColor: CustomHeaderAttribute.HeaderColor.defaultColor)]

        [SerializeField] private TAnimationParametersUI _hoverAnimationParams = new();

        [Space, CustomHeader("Click", count: 3, depth: 0, headerColor: CustomHeaderAttribute.HeaderColor.defaultColor)]

        [SerializeField] private float _clickCooldownTime = 0.5f;
        [SerializeField] private TAnimationParametersUI _clickAnimationParams = new();

        #endregion


        #region Properties

        public float ClickCooldownTime => _clickCooldownTime;
        public TAnimationParametersUI HoverAnimationParams => _hoverAnimationParams;
        public TAnimationParametersUI ClickAnimationParams => _clickAnimationParams;

        #endregion
    }
}
