using System;

namespace UI
{
    public interface IMenuUI
    {
        public GamePanelManagerUI OwnerPanelManager { get; }
        public void Activate(Action activationFinishCallback = default);
        public void RequestDeactivation(Action closeFinishCallback = default, float speedMult = 1f);
    }
}
