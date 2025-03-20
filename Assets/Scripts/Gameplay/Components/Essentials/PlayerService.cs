using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Interfaces;
using Characters.Player;
using Settings;
using Settings.Global;
using Settings.SceneManagement;
using UnityEngine;

namespace Gameplay.Components
{
    public sealed class PlayerService : IService, IDisposable, ICleanable
    {
        #region Fields 

        public event Action<PlayerCharacterController> OnPlayerReady;
        private HashSet<PlayerCharacterController> _players;
        private PlayerEvent _playerEvent;

        #endregion

        public HashSet<PlayerCharacterController> Players => _players;
        public PlayerEvent PlayerEvent => _playerEvent;


        #region Methods

        #region Init

        public PlayerService()
        {
            _players = new HashSet<PlayerCharacterController>();
            _playerEvent = new PlayerEvent();
        }

        public void Dispose()
        {
            _playerEvent?.Dispose();
            Clean();
        }

        public void Clean()
        {
            if (TryGetPlayer(out var player))
            {
                player.OnCharacterDies -= PlayerDiesNotification;
                player.OnCharacterDeathEnd -= PlayerDeadNotification;
            }

            _players.Clear();
        }

        #endregion

        // Note (Future) : This method must be updated to use for multiple players.
        public void AddPlayer(PlayerCharacterController player)
        {
            if (player != null)
            {
                _players.Add(player);
                OnPlayerReady?.Invoke(player);

                player.OnCharacterDies += PlayerDiesNotification;
                player.OnCharacterDeathEnd += PlayerDeadNotification;
            }
        }

        public bool TryGetPlayer(out PlayerCharacterController player)
        {
            player = Players?.FirstOrDefault();

            return player != null;
        }

        private void PlayerDiesNotification(PlayerCharacterController player)
        {
            _playerEvent.ListenEvent(player, new PlayerEventArgs(PlayerEvent.Type.Dies));
        }

        private void PlayerDeadNotification(PlayerCharacterController player)
        {
            _playerEvent.ListenEvent(player, new PlayerEventArgs(PlayerEvent.Type.Dead));
        }

        public void RemovePlayer(PlayerCharacterController player)
        {
            _players.Remove(player);
        }

        public ICharacterLogic GetCharacter()
        {
            // ToDo (future) : implement the logic for several players in the future.

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
}
