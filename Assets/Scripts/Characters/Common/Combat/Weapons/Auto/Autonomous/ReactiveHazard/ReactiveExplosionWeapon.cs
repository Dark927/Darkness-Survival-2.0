using Characters.Common.CustomPhysics2D;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// The universal concrete weapon for all Death-Reaction abilities. 
    /// Features an unlockable Chain Reaction special attack.
    /// </summary>
    public class ReactiveExplosionWeapon : ReactiveHazardWeapon<ReactiveHazardAttackSettings, ReactiveExplosionEntity>, IWeaponWithSpecialAttack
    {
        public bool IsSpecialAttackActive { get; private set; }

        public void EnableSpecialAttack() => IsSpecialAttackActive = true;
        public void DisableSpecialAttack() => IsSpecialAttackActive = false;

        protected override void HandleReactionHit(Collider2D targetCollider, Vector3 explosionPosition)
        {
            // THE CHAIN REACTION (Special Attack Logic)
            if (IsSpecialAttackActive)
            {
                if (targetCollider.TryGetComponent<EntityColliderLink>(out var link) && link.Logic.Body != null)
                {
                    MarkEnemy(link.Logic.Body);
                }
            }

            // THE DAMAGE (Base Logic)
            base.HandleReactionHit(targetCollider, explosionPosition);
        }
    }
}
