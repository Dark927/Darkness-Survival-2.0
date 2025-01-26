namespace Characters.Player.Animation
{
    public class PlayerAnimatorParameters : IAnimatorParameters
    {
        #region Fields

        private string _speedFieldName = "Speed";
        private string _attackTypeFieldName = "AttackType";
        private string _attackTriggerName = "Attack";
        private string _deathTriggerName = "Die";

        #endregion


        #region Properties

        public string SpeedFieldName => _speedFieldName;
        public string AttackTypeFieldName => _attackTypeFieldName;
        public string AttackTriggerName => _attackTriggerName;
        public string DeathTriggerName => _deathTriggerName;

        #endregion
    }
}