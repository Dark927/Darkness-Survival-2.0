using System;
using Gameplay.Components;
using Settings.Global;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _speed = 1f;

    private float _elapsedTime;
    private StageTime _time;
    private bool _isStopped;

    public event Action<StageTime> OnTimeChanged;

    #endregion

    #region Properties

    public StageTime CurrentTime => _time;

    #endregion


    #region Methods

    private void Awake()
    {
        Reset();
        Stop();
    }

    private void Update()
    {
        if (_isStopped)
        {
            return;
        }

        _elapsedTime += Time.deltaTime * _speed;

        if (_time.TryUpdateSeconds((uint)Mathf.FloorToInt(_elapsedTime)))
        {
            OnTimeChanged?.Invoke(_time);
        }
    }

    public void Stop()
    {
        _isStopped = true;
    }

    public void Activate()
    {
        _isStopped = false;
    }

    public void Reset()
    {
        _elapsedTime = 0;
    }

    #endregion
}
