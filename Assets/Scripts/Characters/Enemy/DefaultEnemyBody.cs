using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class DefaultEnemyBody : CharacterBody
{
    #region Fields

    private EnemyAnimatorController _animatorController;
    [SerializeField] private int _sideSwitchDelayInMs = 0;
    [SerializeField] private int _accelerationTimeInMs = 500;
    private bool _isWaiting = false;

    private UniTask _activeSideSwitch;
    private CancellationTokenSource _cancellationTokenSource;

    #endregion


    #region Properties

    public EnemyAnimatorController AnimatorController { get => _animatorController; private set => _animatorController = value; }
    public bool Waiting { get => _isWaiting; private set => _isWaiting = value; }

    #endregion


    #region Methods

    #region Init

    protected override void InitView()
    {
        View = new CharacterLookDirection(transform);
    }

    protected override void InitMovement()
    {
        Movement = new EnemyMovement(this, FindObjectOfType<PlayerBody>());
    }

    protected override void InitAnimation()
    {
        Animator animator = GetComponent<Animator>();
        AnimatorController = new EnemyAnimatorController(animator, new EnemyAnimatorParameters());
    }

    protected override void InitReferences()
    {

    }

    #endregion

    private void FixedUpdate()
    {
        if (!Waiting)
        {
            Movement.Move();

            if (!View.IsLookingForward(Movement.Direction))
            {
                RequestSideSwitch();
            }
        }
    }

    private void RequestSideSwitch()
    {
        if (_activeSideSwitch.Status == UniTaskStatus.Pending)
        {
            InterruptCurrentSideSwitch();
        }
        else
        {
            _activeSideSwitch = SideSwitch(_sideSwitchDelayInMs);
        }
    }

    private async UniTask SideSwitch(int delayInMs)
    {
        Waiting = true;
        Movement.BlockMovement(delayInMs);

        _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = _cancellationTokenSource.Token;

        await UniTask.Delay(delayInMs, cancellationToken: token);

        Waiting = false;

        if (token.IsCancellationRequested)
        {
            return;
        }

        View.LookForward(Movement.Direction);

        await Movement.UpdateSpeedMultiplierLinear(Movement.MaxSpeedMultiplier, _accelerationTimeInMs, token);

    }

    private void InterruptCurrentSideSwitch()
    {
        _cancellationTokenSource?.Cancel();
    }

    #endregion

}
