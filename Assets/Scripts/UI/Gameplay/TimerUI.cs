using Gameplay.Components;
using TMPro;
using UnityEngine;
using Utilities.ErrorHandling;
using Zenject;

public class TimerUI : MonoBehaviour
{
    #region Fields

    private TextMeshProUGUI _textMesh;
    private GameTimer _timer;

    #endregion


    #region Methods

    #region Init 

    [Inject]
    public void Construct(GameTimer timer)
    {
        _timer = timer;
    }

    private void Awake()
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (_timer == null)
        {
            ErrorLogger.LogComponentIsNull(LogOutputType.Console, gameObject.name, nameof(_timer));
            gameObject.SetActive(false);
        }

        _timer.OnTimeChanged += UpdateUI;
    }

    #endregion

    private void UpdateUI(StageTime time)
    {
        _textMesh.text = $"{time.Minutes:00}:{time.Seconds:00}";
    }

    #endregion
}
