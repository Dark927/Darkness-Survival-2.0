using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

public class EntityActionBlock
{
    #region Fields 

    private UniTask _activeBlockTask;
    private CancellationTokenSource _cancellationTokenSource;
    private bool _isBlocked = false;

    #endregion


    #region Properties

    public event Action OnBlockFinish;
    public bool IsBlocked => _isBlocked;

    #endregion


    #region Methods

    #region Init

    public EntityActionBlock()
    {
    }

    #endregion


    public void Block(int timeInMs)
    {
        if (timeInMs <= 0)
        {
            return;
        }

        Unblock();
        Block();

        _cancellationTokenSource = new CancellationTokenSource();
        _activeBlockTask = BlockDelayTask(timeInMs, _cancellationTokenSource.Token);
    }

    public void Block()
    {
        _isBlocked = true;
    }

    public void Unblock()
    {
        if (_activeBlockTask.Status == UniTaskStatus.Pending)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        _isBlocked = false;
    }

    private async UniTask BlockDelayTask(int timeInMs, CancellationToken token)
    {
        await Task.Delay(timeInMs, cancellationToken: token);
        OnBlockFinish?.Invoke();

        _activeBlockTask = UniTask.CompletedTask;
        Unblock();
    }

    #endregion

}
