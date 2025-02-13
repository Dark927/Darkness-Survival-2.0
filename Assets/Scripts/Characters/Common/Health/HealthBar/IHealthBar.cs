namespace Characters.Health.HealthBar
{
    public interface IHealthBar
    {
        public void Hide();
        public void Show();
        public void UpdateActualHp(float actualHpPercent);
    }
}