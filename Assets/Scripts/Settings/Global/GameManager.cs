using Characters.Player;
using Settings.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

// # Info : Component has a configured execution order in the Project Settings, Order : -1. 
public class GameManager : SingletonBase<GameManager>
{
    #region Fields

    public event Action<Player> OnPlayerReady;
    public event Action OnStageStarted;
    private List<Player> _players;

    #endregion


    #region Methods

    #region Init

    private void Awake()
    {
        InitInstance(true);
    }

    #endregion

    public void StartStage()
    {
        OnStageStarted?.Invoke();
    }

    public void SetPlayers(List<Player> players)
    {
        _players = players;
        OnPlayerReady?.Invoke(_players.FirstOrDefault());
    }

    #endregion
}
