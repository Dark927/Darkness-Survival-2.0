using UnityEngine;

namespace Characters.Common.Combat.Weapons.Data
{
    [CreateAssetMenu(fileName = "NewWeaponAttackData", menuName = "Game/Combat/Data/Attacks/Weapon Attack Data")]
    public class WeaponAttackData : ScriptableObject
    {
        [SubclassSelector]
        [SerializeReference]
        private IAttackSettings _settings;

        public IAttackSettings Settings => _settings;
    }
}
