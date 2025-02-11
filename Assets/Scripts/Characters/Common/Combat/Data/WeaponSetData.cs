using System.Collections.Generic;
using UnityEngine;

namespace Characters.Common.Combat.Weapons.Data
{
    [CreateAssetMenu(fileName = "NewWeaponSetData", menuName = "Game/Characters/Data/Weapons/WeaponSetData")]
    public class WeaponSetData : ScriptableObject
    {
        [Space, Header("Main Settings")]

        [SerializeField] private string _weaponsContainerName;
        [SerializeField] private List<EntityWeaponData> _basicWeapons;

        public string ContainerName => _weaponsContainerName;
        public List<EntityWeaponData> BasicWeapons => _basicWeapons;
    }
}
