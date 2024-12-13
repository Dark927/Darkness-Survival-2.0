
using System;
using System.Threading.Tasks;
using UnityEngine;

public class CharacterMovementBlock
{
    #region Fields 

    private Task _activeBlockTask;
    private ICharacterMovement _movement;
    private bool _isBlocked = false;

    #endregion


    #region Properties

    public event Action OnBlockFinish;
    public bool IsBlocked { get => _isBlocked; private set => _isBlocked = value; }

    #endregion


    #region Methods

    #region Init

    public CharacterMovementBlock(ICharacterMovement movement)
    {
        _movement = movement;
    }

    #endregion


    public void BlockMovement(int timeInMs)
    {
        bool hasActiveBlock = (_activeBlockTask != null) && !_activeBlockTask.IsCompleted;
        bool incorrectTime = timeInMs <= 0;

        if (incorrectTime || hasActiveBlock)
        {
            return;
        }

        _movement.Stop();
        _activeBlockTask = MovementStopDelay(timeInMs);
    }

    private async Task MovementStopDelay(int timeInMs)
    {
        IsBlocked = true;

        await Task.Delay(timeInMs);

        IsBlocked = false;
        _activeBlockTask = null;
        OnBlockFinish?.Invoke();
    }

    #endregion

}
