using Characters.Common.Features;
using Characters.Interfaces;
using Settings.Global;

namespace UI.Local.Health
{
    public interface IHealthBar : IEntityFeature, IEventListener
    {
        public void Hide();
        public void Show();
        public void SetCharacterBody(IEntityBody characterBody);
        public void UpdateActualHp(float actualHpPercent);
    }
}
