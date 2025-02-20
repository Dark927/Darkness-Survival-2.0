using Characters.Common.Movement;
using Characters.Common.Physics2D;
using Characters.Health;
using System;


namespace Characters.Interfaces
{
    public interface IEntityPhysicsBody : IEntityBody
    {
        #region Properties

        public IEntityPhysics2D Physics { get; }
        public IHealth Health { get; }
        public IInvincibility Invincibility { get; }
        public IEntityMovement Movement { get; }
        public bool IsDying { get; }

        #endregion


        #region Events

        /// <summary>
        /// Used to notify that the character's body is dying (but not yet dead).
        /// </summary>
        public event Action OnBodyDies;

        /// <summary>
        /// Used to notify that the character's body is comletely dead.
        /// </summary>
        public event Action OnBodyDiedCompletely;

        public event Action OnBodyDamaged;


        #endregion
    }
}
