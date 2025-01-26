
using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using System.Threading.Tasks;

public class CharacterMovementBlock
{
    #region Fields 

    private UniTask _activeBlockTask;
    private ICharacterMovement _movement;

    #endregion


    #region Properties

    public event Action OnBlockFinish;
    public bool IsBlocked { get => (_activeBlockTask.Status == UniTaskStatus.Pending); }

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
        bool incorrectTime = timeInMs <= 0;

        if (incorrectTime || IsBlocked)
        {
            return;
        }

        _movement.Stop();
        _activeBlockTask = MovementStopDelay(timeInMs);
    }

    private async UniTask MovementStopDelay(int timeInMs)
    {
        await Task.Delay(timeInMs);
        OnBlockFinish?.Invoke();
    }

    #endregion

}
