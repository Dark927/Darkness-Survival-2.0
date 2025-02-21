using UnityEngine;

namespace Characters.Common.Combat.Weapons.Data
{
    [CreateAssetMenu(fileName = "NewAttackNegativeStatusData", menuName = "Game/Combat/Data/Attacks/AttackNegativeStatusData")]
    public class AttackNegativeStatusData : ScriptableObject
    {
        #region Fields

        [Space, Header("Main Settings")]
        [SerializeField] private string _name;
        [SerializeField, TextArea(2, 3)] private string _description;

        [Space, Header("Effect Settings")]

        [SerializeField] private AttackNegativeStatus _settings = AttackNegativeStatus.Zero;

        #endregion


        #region Properties

        public AttackNegativeStatus Settings => _settings;
        public string Name => _name;
        public string Description => _description;

        #endregion
    }
}
