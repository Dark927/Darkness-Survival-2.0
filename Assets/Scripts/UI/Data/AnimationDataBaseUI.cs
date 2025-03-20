
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

        [Space, CustomHeader("Click", count: 2, depth: 0, headerColor: CustomHeaderAttribute.HeaderColor.defaultColor)]

        [SerializeField] private TAnimationParametersUI _clickAnimationParams = new();

        #endregion


        #region Properties

        public TAnimationParametersUI HoverAnimationParams => _hoverAnimationParams;
        public TAnimationParametersUI ClickAnimationParams => _clickAnimationParams;

        #endregion
    }
}
