using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CharacterActionBlock
{
    #region Fields 

    private UniTask _activeBlockTask;
    private CancellationTokenSource _cancellationTokenSource;

    #endregion


    #region Properties

    public event Action OnBlockFinish;
    public bool IsBlocked { get => (_activeBlockTask.Status == UniTaskStatus.Pending); }

    #endregion


    #region Methods

    #region Init

    public CharacterActionBlock()
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

        _cancellationTokenSource = new CancellationTokenSource();
        _activeBlockTask = BlockDelay(timeInMs, _cancellationTokenSource.Token);
    }

    public void Unblock()
    {
        if (IsBlocked)
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }
    }

    private async UniTask BlockDelay(int timeInMs, CancellationToken token)
    {
        await Task.Delay(timeInMs, cancellationToken: token);
        OnBlockFinish?.Invoke();
    }

    #endregion

}
