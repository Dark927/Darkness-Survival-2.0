using Characters.Common.Features;
using Characters.Interfaces;
using Settings.Global;

namespace UI.Local.Health
{
    public interface IHealthBar : IEntityFeature, IEventsConfigurable
    {
        public void Hide();
        public void Show();
        public void SetCharacterBody(IEntityPhysicsBody characterBody);
        public void UpdateActualHp(float actualHpPercent);
    }
}
