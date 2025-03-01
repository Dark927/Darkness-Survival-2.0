using Settings.Global;
using System;


namespace Characters.Common.Combat
{
    public interface IAttackTrigger : IDisposable, IInitializable
    {
        public event EventHandler OnTriggerEnter;
        public event EventHandler OnTriggerStay;
        public event EventHandler OnTriggerExit;
        public event Action<IAttackTrigger> OnTriggerDeactivation;

        public void Activate();
        public void Activate(float timeInSec);
        public void Deactivate();
    }
}
