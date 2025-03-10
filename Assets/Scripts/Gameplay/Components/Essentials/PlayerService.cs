using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Interfaces;
using Characters.Player;
using Settings.Global;
using UnityEngine;

public sealed class PlayerService : IService
{
    #region Fields 

    public event Action<PlayerCharacterController> OnPlayerReady;
    private HashSet<PlayerCharacterController> _players;

    #endregion

    public HashSet<PlayerCharacterController> Players => _players;


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

            // ToDo : clear player list only when stage is restarted (or move this service to the gameplay essentials)
            _players.RemoveWhere(player => player == null);

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
        return (player != null) ? player.CharacterLogic : null;
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
