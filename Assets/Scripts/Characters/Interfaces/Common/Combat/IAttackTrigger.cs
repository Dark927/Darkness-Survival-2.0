using System;
using Settings.Global;


namespace Characters.Common.Combat
{
    public interface IAttackTrigger : IDisposable, IInitializable
    {
        public float CurrentActivationTime { get; }

        public event EventHandler OnTriggerEnter;
        public event EventHandler OnTriggerStay;
        public event EventHandler OnTriggerExit;
        public event Action<IAttackTrigger> OnTriggerDeactivation;
        public event Action<IAttackTrigger> OnTriggerActivation;

        public void Activate();
        public void Activate(float timeInSec);
        public void Deactivate();
    }
}
