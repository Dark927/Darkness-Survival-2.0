using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollJumpFixerUI : MonoBehaviour
{
    private ScrollRect _scrollRect;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
    }

    /// <summary>
    /// Forces the Canvas to update its layout immediately and snaps the scroll bar to the top.
    /// </summary>
    public void SnapToTop()
    {
        Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 1f;
    }
}
