
namespace UI.Buttons
{
    public class ClosePanelButtonUI : ButtonBaseUI
    {
        public override void Click()
        {
            ParentMenu.RequestDeactivation();
        }
    }
}
