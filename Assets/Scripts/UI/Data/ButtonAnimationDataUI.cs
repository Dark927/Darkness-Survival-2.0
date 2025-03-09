
using Settings;
using UnityEngine;
using Utilities.Attributes;

namespace UI.Buttons
{
    [CreateAssetMenu(fileName = "UI_NewButtonAnimation_Data", menuName = "Game/UI/Animations/Button Animation Data")]
    public class ButtonAnimationDataUI : DescriptionBaseData
    {
        #region Fields 

        [CustomHeader("Hover", count: 2, depth: 0, headerColor: CustomHeaderAttribute.HeaderColor.defaultColor)]

        [SerializeField] private ButtonAnimationParamsUI _hoverAnimationParams = new ButtonAnimationParamsUI();

        [Space, CustomHeader("Click", count: 2, depth: 0, headerColor: CustomHeaderAttribute.HeaderColor.defaultColor)]

        [SerializeField] private ButtonAnimationParamsUI _clickAnimationParams = new ButtonAnimationParamsUI();

        #endregion


        #region Properties

        public ButtonAnimationParamsUI HoverAnimationParams => _hoverAnimationParams;
        public ButtonAnimationParamsUI ClickAnimationParams => _clickAnimationParams;

        #endregion
    }
}
