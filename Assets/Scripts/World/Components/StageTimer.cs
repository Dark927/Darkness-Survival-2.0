using System;
using UnityEngine;

public class StageTimer : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _speed = 1f;
    
    private float _timer;
    private int _elapsedSeconds;

    public event Action<int> OnSecondChanged;

    #endregion


    #region Properties

    public int ElapsedSeconds
    {
        get => _elapsedSeconds;
        set
        {
            if(value != _elapsedSeconds)
            {
                _elapsedSeconds = value;
                OnSecondChanged?.Invoke(value);
            }
        }
    }

    #endregion


    #region Methods
    
    private void Awake()
    {
        _timer = 0;
    }

    private void Update()
    {
        _timer += Time.deltaTime * _speed;
        ElapsedSeconds = Mathf.FloorToInt(_timer);
    }

    public void Reset()
    {
        _timer = 0;
        ElapsedSeconds = 0;
    }

    #endregion
}
