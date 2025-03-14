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
            ICharacterLogic character = GetCharacter();

            if (character != null)
            {
                character.Body.OnBodyDies -= PlayerDiesNotification;
                character.Body.OnBodyDiedCompletely -= PlayerDeadNotification;
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

                player.CharacterLogic.Body.OnBodyDies += PlayerDiesNotification;
                player.CharacterLogic.Body.OnBodyDiedCompletely += PlayerDeadNotification;
            }
        }

        public bool TryGetPlayer(out PlayerCharacterController player)
        {
            player = Players?.FirstOrDefault();

            return player != null;
        }

        private void PlayerDiesNotification()
        {
            _playerEvent.ListenEvent(this, new PlayerEventArgs(PlayerEvent.Type.Dies));
        }

        private void PlayerDeadNotification()
        {
            _playerEvent.ListenEvent(this, new PlayerEventArgs(PlayerEvent.Type.Dead));
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
