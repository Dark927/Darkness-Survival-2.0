using System;
using UnityEngine;
using World.Components;

public class GameTimer : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _speed = 1f;

    private float _elapsedTime;
    private StageTime _time;

    public event Action<StageTime> OnTimeChanged;

    #endregion

    #region Properties

    public StageTime CurrentTime => _time;

    #endregion


    #region Methods

    private void Awake()
    {
        _elapsedTime = 0;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime * _speed;

        if (_time.TryUpdateSeconds((uint)Mathf.FloorToInt(_elapsedTime)))
        {
            OnTimeChanged?.Invoke(_time);
        }
    }

    public void Reset()
    {
        _elapsedTime = 0;

    }

    #endregion
}
