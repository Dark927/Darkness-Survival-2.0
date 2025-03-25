
using System;

namespace Characters.Player
{
    public class NeroVisual : PlayerCharacterVisual
    {
        private NeroSwordVisual _swordVisual;

        public override void Initialize()
        {
            base.Initialize();
            _swordVisual = GetComponentInChildren<NeroSwordVisual>(true);
            _swordVisual.Initialize();
        }


        public void EnableSword()
        {
            DoSwordAction(_swordVisual.Activate);
        }

        public void DisableSword()
        {
            DoSwordAction(_swordVisual.Deactivate);
        }

        private void DoSwordAction(Action action)
        {
            if (_swordVisual == null)
            {
                return;
            }

            action();
        }
    }
}
