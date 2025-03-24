using System.Collections.Generic;
using Characters.Interfaces;
using Characters.Player.Upgrades;
using UnityEngine;


namespace Characters.Player.Upgrades
{

    [CreateAssetMenu(fileName = "WeaponUnlockLevelData", menuName = "Game/Upgrades/Weapon Upgrades/Unlock/Weapon Unlock Level Data")]

    public class WeaponUnlockLevelSO : UpgradeLevelSO<ICharacterLogic>
    {

    }
}
