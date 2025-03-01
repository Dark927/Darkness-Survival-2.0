using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class EntityActionBlock
{
    #region Fields 

    private int _activeBlockCount = 0;
    private UniTask _activeBlockTask;
    private CancellationTokenSource _cancellationTokenSource;

    #endregion

    #region Properties

    public event Action OnBlockFinish;
    public bool IsBlocked => _activeBlockCount > 0; // Blocked if any active block exists

    #endregion

    #region Methods


    // Block with time (temporary block)
    public void Block(int timeInMs)
    {
        if (timeInMs <= 0) return;

        // Block and start a temporary block task

        CancelBlockTask();
        Block();
        _cancellationTokenSource = new CancellationTokenSource();
        _activeBlockTask = BlockDelayTask(timeInMs, _cancellationTokenSource.Token);
    }
    // Permanent block (adds to active block count)
    public void Block()
    {
        _activeBlockCount++;
    }

    // Unblock - decrease the active block count
    public void Unblock(bool disableTimerBlocks = false)
    {
        if (_activeBlockCount > 0)
        {
            _activeBlockCount--;

            if (disableTimerBlocks)
            {
                CancelBlockTask();
            }
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

    // Handle temporary block task (delayed unblock)
    private async UniTask BlockDelayTask(int timeInMs, CancellationToken token)
    {
        await UniTask.Delay(timeInMs, cancellationToken: token);
        OnBlockFinish?.Invoke();

        Unblock();
    }

    #endregion
}
