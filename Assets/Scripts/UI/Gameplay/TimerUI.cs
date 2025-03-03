using Gameplay.Components;
using TMPro;
using UnityEngine;
using Utilities.ErrorHandling;

public class TimerUI : MonoBehaviour
{
    #region Fields

    private TextMeshProUGUI _textMesh;
    private GameTimer _timer;

    #endregion


    #region Methods

    #region Init 

    private void Awake()
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();

        _timer = FindAnyObjectByType<GameTimer>();

        if (_timer == null)
        {
            ErrorLogger.LogComponentIsNull(LogOutputType.Console, gameObject.name, nameof(_timer));
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        _timer.OnTimeChanged += UpdateUI;
    }

    #endregion

    private void UpdateUI(StageTime time)
    {
        _textMesh.text = $"{time.Minutes:00}:{time.Seconds:00}";
    }

    #endregion
}
