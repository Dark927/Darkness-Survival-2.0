using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class EntityActionBlock
{
    #region Fields 

    private int _permanentBlockCount = 0;
    private bool _isTimerBlocked = false;

    private CancellationTokenSource _cancellationTokenSource;

    #endregion

    #region Properties

    public event Action OnBlockFinish;

    // Blocked if there is any permanent block OR an active timer block
    public bool IsBlocked => _permanentBlockCount > 0 || _isTimerBlocked;

    #endregion

    #region Methods

    public void Block(int timeInMs)
    {
        if (timeInMs <= 0)
        {
            return;
        }

        CancelBlockTask();

        _isTimerBlocked = true;

        _cancellationTokenSource = new CancellationTokenSource();
        BlockDelayTask(timeInMs, _cancellationTokenSource.Token).Forget();
    }

    public void Block()
    {
        _permanentBlockCount++;
    }

    public void Unblock(bool disableTimerBlocks = false)
    {
        if (_permanentBlockCount > 0)
        {
            _permanentBlockCount--;
        }

        if (disableTimerBlocks)
        {
            CancelBlockTask();
            _isTimerBlocked = false;
        }
    }

    private void CancelBlockTask()
    {
        if (_cancellationTokenSource == null)
        {
            return;
        }

        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _cancellationTokenSource = null;
    }

    private async UniTaskVoid BlockDelayTask(int timeInMs, CancellationToken token)
    {
        bool isCanceled = await UniTask.Delay(timeInMs, cancellationToken: token).SuppressCancellationThrow();

        if (isCanceled)
        {
            return;
        }

        _isTimerBlocked = false;
        OnBlockFinish?.Invoke();
    }

    #endregion
}
