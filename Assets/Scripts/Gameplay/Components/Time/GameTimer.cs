using System;
using Gameplay.Components;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    #region Events

    public event Action<StageTime> OnTimeChanged;

    #endregion


    #region Fields

    [SerializeField] private float _speed = 1f;

    private float _nextSecondThreshold;
    private float _elapsedTime;
    private StageTime _time;

    #endregion

    #region Properties

    public StageTime CurrentStageTime => _time;
    public float ElapsedTime => _elapsedTime;

    #endregion


    #region Methods

    private void Awake()
    {
        Reset();
    }

    public void UpdateTime()
    {
        _elapsedTime += Time.deltaTime * _speed;

        if (_elapsedTime >= _nextSecondThreshold)
        {
            _nextSecondThreshold = Mathf.Floor(_elapsedTime) + 1f;
            _time.TryUpdateSeconds((uint)(_nextSecondThreshold - 1f));
            OnTimeChanged?.Invoke(_time);
        }
    }

    public void Reset()
    {
        _elapsedTime = 0;
        _nextSecondThreshold = 0f;
    }

    #endregion
}
