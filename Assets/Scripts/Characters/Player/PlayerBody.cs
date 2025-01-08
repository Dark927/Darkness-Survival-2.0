public class PlayerBody : CharacterBody
{
    #region Fields

    private IPlayerLogic _playerLogic;

    #endregion


    #region Properties


    #endregion


    #region Methods 

    #region Init

    protected override void Init()
    {
        Visual = GetComponentInChildren<PlayerVisual>();
    }

    protected override void InitView()
    {
        View = new CharacterLookDirection(transform);
    }

    protected override void InitMovement()
    {
        Movement = new PlayerMovement(this);
        CharacterSpeed speed = new CharacterSpeed() { CurrentSpeedMultiplier = 4, MaxSpeedMultiplier = 4 };
        Movement.Speed.Set(speed);
    }

    protected override void InitReferences()
    {
        Movement.Speed.OnActualSpeedChanged += (Visual.GetAnimatorController() as PlayerAnimatorController).SpeedUpdateListener;
    }

    protected override void ClearReferences()
    {
        Movement.Speed.OnActualSpeedChanged -= (Visual.GetAnimatorController() as PlayerAnimatorController).SpeedUpdateListener;
    }

    #endregion


    private void FixedUpdate()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        Movement?.Move();
        View?.LookForward(Movement.Direction);
    }

    #endregion

}
