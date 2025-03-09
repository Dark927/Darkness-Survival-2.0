
using UnityEngine;

namespace UI.CustomCursor
{
    [CreateAssetMenu(fileName = "UI_NewCursor_Data", menuName = "Game/UI/Global/Cursor Data")]
    public class CustomCursorData : ScriptableObject
    {
        [SerializeField] private Sprite _defaultState;
        [SerializeField] private Sprite _hoverUIState;

        [SerializeField] private DefaultAnimationParamsUI _hoverParams = new DefaultAnimationParamsUI();

        public Sprite DefaultState => _defaultState;
        public Sprite HoverUIState => _hoverUIState;
        public DefaultAnimationParamsUI HoverParams => _hoverParams;
    }
}
