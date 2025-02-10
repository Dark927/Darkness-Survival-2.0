using Settings.Abstract;
using UnityEngine;

public sealed class GameplayUI : SingletonBase<GameplayUI>
{
    [SerializeField] private GameObject _gameOverPanel;

    private void Awake()
    {
        InitInstance();
    }

    public void ActivateGameOverPanel()
    {
        _gameOverPanel.SetActive(true);
    }
}