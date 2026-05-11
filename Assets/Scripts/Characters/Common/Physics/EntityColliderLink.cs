using UnityEngine;

namespace Characters.Common.CustomPhysics2D
{
    /// <summary>
    /// Attach to the GameObject containing the Collider2D.
    /// Provides an O(1) instant bridge to the main Entity Logic.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class EntityColliderLink : MonoBehaviour
    {
        [SerializeField] private AttackableEntityLogicBase _mainLogic;

        public IEntityDynamicLogic Logic => _mainLogic;
    }
}
