using Characters.Common;
using Characters.Common.Features;
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
