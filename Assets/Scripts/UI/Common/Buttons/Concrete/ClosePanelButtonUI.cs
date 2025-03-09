namespace UI.Buttons
{
    public class ClosePanelButtonUI : ButtonBaseUI
    {
        public override void Click()
        {
            GamePanelManagerUI.Instance.CloseCurrentPanel();
        }
    }
}
