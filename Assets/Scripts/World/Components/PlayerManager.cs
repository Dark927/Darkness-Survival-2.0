using Characters.Interfaces;
using Characters.Player;
using Settings.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public sealed class PlayerManager : IService
{
    #region Fields 

    public event Action<PlayerCharacterController> OnPlayerReady;
    private HashSet<PlayerCharacterController> _players;

    #endregion


    #region Methods

    #region Init

    public void Init()
    {
        _players = new HashSet<PlayerCharacterController>();
    }

    #endregion

    public void AddPlayer(PlayerCharacterController player)
    {
        if (player != null)
        {
            _players.Add(player);
            OnPlayerReady?.Invoke(player);

            // ToDo : move this logic to the another manager
            GameplayUI.Instance.Initialize(player);
        }
    }

    public void RemovePlayer(PlayerCharacterController player)
    {
        _players.Remove(player);
    }

    public ICharacterLogic GetCharacter()
    {
        // ToDo : implement the logic for several players in the future.

        PlayerCharacterController player = _players.FirstOrDefault();
        return (player != null) ? player.Character : null;
    }

    public Transform GetCharacterTransform()
    {
        Transform characterTransform = null;
        ICharacterLogic character = GetCharacter();

        if (character != null)
        {
            characterTransform = (character.Body != null) ? character.Body.Transform : null;
        }

        return characterTransform;
    }

    #endregion
}
