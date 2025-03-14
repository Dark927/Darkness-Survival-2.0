using Settings.Global;
using Settings.SceneManagement;

namespace UI.Buttons
{
    public sealed class StartStageButtonUI : ButtonBaseUI
    {
        private GameStateService _gameStateService;

        protected override void Awake()
        {
            base.Awake();
            _gameStateService = ServiceLocator.Current?.Get<GameStateService>();
        }

        public override void Click()
        {
            if (_gameStateService != null)
            {
                _gameStateService.StartStage();
            }
        }
    }
}
