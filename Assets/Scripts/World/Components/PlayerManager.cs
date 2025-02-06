using Characters.Player;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Settings.Global;
using System;
using Characters.Interfaces;

public sealed class PlayerManager : IService
{
    #region Fields 

    public event Action<Player> OnPlayerReady;
    private HashSet<Player> _players;

    #endregion


    #region Methods

    #region Init

    public void Init()
    {
        _players = new HashSet<Player>();
    }

    #endregion

    public void AddPlayer(Player player)
    {
        if (player != null)
        {
            _players.Add(player);
            OnPlayerReady?.Invoke(player);
        }
    }

    public void RemovePlayer(Player player)
    {
        _players.Remove(player);
    }

    public ICharacterLogic GetCharacter()
    {
        // ToDo : implement the logic for several players in the future.

        Player player = _players.FirstOrDefault();
        return (player != null) ? player.Character : null;
    }

    public Transform GetCharacterTransform()
    {
        Transform characterTransform = null;
        ICharacterLogic character = GetCharacter();

        if (character != null)
        {
            characterTransform = (character.Body != null) ? character.Body.transform : null;
        }

        return characterTransform;
    }

    #endregion
}
