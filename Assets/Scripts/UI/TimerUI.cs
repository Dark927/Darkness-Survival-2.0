using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    #region Fields

    private TextMeshProUGUI _textMesh;
    private StageTimer _timer;

    #endregion


    #region Methods

    #region Init 
    
    private void Awake()
    {
        _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        _timer = FindAnyObjectByType<StageTimer>();

        if (_timer == null)
        {
            ErrorHandling.ErrorLogger.LogComponentIsNull(ErrorHandling.LogOutputType.Console, gameObject.name, nameof(_timer));
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        _timer.OnSecondChanged += UpdateUI;
    }

    #endregion

    private void UpdateUI(int timeInSec)
    {
        int minutes = Mathf.FloorToInt(timeInSec / 60f);
        int seconds = Mathf.FloorToInt(timeInSec % 60f);

        _textMesh.text = $"{minutes:00}:{seconds:00}";
    }

    #endregion
}
