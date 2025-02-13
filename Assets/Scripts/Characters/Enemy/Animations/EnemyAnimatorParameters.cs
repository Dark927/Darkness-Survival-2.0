namespace Characters.Enemy.Animation
{
    public class EnemyAnimatorParameters : IAnimatorParameters
    {
        #region Fields

        private string _speedFieldName = "Speed";

        #endregion


        #region Properties

        public string SpeedFieldName => _speedFieldName;

        #endregion
    }
}