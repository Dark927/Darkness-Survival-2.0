using Characters.Player.Weapons.Data;
using Characters.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Data
{
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game/Characters/Data/PlayerData")]
    public class PlayerCharacterData : CharacterBaseData
    {
        [Space, Header("Weapons Settings")]

        //[SerializeField] private GameObject _weaponsContainer;
        [SerializeField] private List<CharacterWeaponData> _basicWeapons;

        //public GameObject WeaponsContainer => _weaponsContainer;
        public List<CharacterWeaponData> BasicWeapons => _basicWeapons;
    }
}