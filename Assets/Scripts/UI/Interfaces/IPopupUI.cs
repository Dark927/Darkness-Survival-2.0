

using System;

namespace UI
{
    public interface IPopupUI
    {
        public void PrepareAnimation();
        public void Show(Action callback = null);
        public void Hide(Action callback = null);
    }
}
