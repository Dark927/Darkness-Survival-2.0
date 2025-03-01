using Characters.Common.Visual;


namespace Gameplay.Stage.Objects
{
    public class ActivatableAnimatorParameters : IAnimatorParameters
    {
        #region Fields

        private string _activationFieldName = "Activate";
        private string _deactivationFieldName = "Deactivate";

        #endregion


        #region Properties

        public string ActivationFieldName => _activationFieldName;
        public string DeactivationFieldName => _deactivationFieldName;

        #endregion
    }
}
