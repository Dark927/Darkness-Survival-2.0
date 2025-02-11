
using Characters.Common.Movement;
using Characters.Common.Visual;
using Characters.Health;
using Settings.Global;
using System;
using UnityEngine;

namespace Characters.Interfaces
{
    public interface ICharacterBody : IEventListener, IResetable, IDisposable
    {
        /// <summary>
        /// Used to notify that the character's body is dying (but not yet dead).
        /// </summary>
        public event Action OnBodyDies;

        /// <summary>
        /// Used to notify that the character's body is comletely dead.
        /// </summary>
        public event Action OnBodyDied;

        public event Action OnBodyDamaged;

        public Transform Transform { get; }
        public CharacterMovementBase Movement { get; }
        public ICharacterView View { get; }
        public EntityVisualBase Visual { get; }
        public IHealth Health { get; }
        public IInvincibility Invincibility { get; }
        public bool IsDead { get; }
    }
}
