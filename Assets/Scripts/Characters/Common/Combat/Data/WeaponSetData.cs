using Characters.Player.Weapons.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Characters.Common.Combat.Weapons.Data
{
    [CreateAssetMenu(fileName = "NewWeaponSetData", menuName = "Game/Characters/Data/Weapons/WeaponSetData")]
    public class WeaponSetData : ScriptableObject
    {
        [Space, Header("Main Settings")]

        [SerializeField] private string _weaponsContainerName;
        [SerializeField] private List<CharacterWeaponData> _basicWeapons;

        public string ContainerName => _weaponsContainerName;
        public List<CharacterWeaponData> BasicWeapons => _basicWeapons;
    }
}
