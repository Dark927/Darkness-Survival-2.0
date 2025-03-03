namespace UI.Buttons
{
    public class ClosePanelButtonUI : ButtonBaseUI
    {
        public override void ClickListener()
        {
            GamePanelManager.Instance.CloseCurrentPanel();
        }
    }
}
