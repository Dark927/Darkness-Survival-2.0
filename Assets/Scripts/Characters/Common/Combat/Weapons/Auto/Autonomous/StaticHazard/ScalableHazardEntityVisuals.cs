
namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// Universal visual handler for all Static Hazards (Puddles, Traps, etc.).
    /// Automatically scales the base and extra visuals to match the physical hazard radius.
    /// </summary>
    public class ScalableHazardEntityVisuals : AutonomousEntityVisuals
    {
        private IResizableAutonomousEntity _hazardLogic;

        protected override void Awake()
        {
            base.Awake();
            if (_entityLogic != null && _entityLogic is IResizableAutonomousEntity hazardEntityLogic)
            {
                _hazardLogic = hazardEntityLogic;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable(); // standard Show/Hide events

            if (_hazardLogic != null)
            {
                _hazardLogic.OnRadiusUpdated += SetRadius;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_hazardLogic != null)
            {
                _hazardLogic.OnRadiusUpdated -= SetRadius;
            }
        }
    }
}
