using Characters.Interfaces;
using Settings.Global;

namespace UI.Local.Health
{
    public interface IHealthBar : IEventListener
    {
        public void Hide();
        public void Show();
        public void Initialize(ICharacterBody characterBody);
        public void SetCharacter(ICharacterBody characterBody);
        public void UpdateActualHp(float actualHpPercent);
    }
}
