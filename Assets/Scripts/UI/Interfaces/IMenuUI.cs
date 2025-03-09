using System;

namespace Assets
{
    public interface IMenuUI
    {
        public void Activate(Action callback = default);
        public void Deactivate(Action callback = default, float speedMult = 1f);
    }
}
