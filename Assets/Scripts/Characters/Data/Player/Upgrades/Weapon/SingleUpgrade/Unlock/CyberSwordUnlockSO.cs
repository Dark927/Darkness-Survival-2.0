
using Characters.Interfaces;
using Characters.Player;
using Characters.Player.Upgrades;
using UnityEngine;

namespace Assets.Scripts.Characters.Data.Player.Upgrades.Weapon.SingleUpgrade.Unlock
{
    /// <summary>
    /// This weapon is special for Nero character (with special animations set)
    /// </summary>
    [CreateAssetMenu(fileName = "NewCyberSwordUnlockData", menuName = "Game/Upgrades/Weapon Upgrades/Unlock/Cyber Sword Unlock Data")]
    public class CyberSwordUnlockSO : SingleWeaponUnlockSO
    {
        public override void ApplyUpgrade(IUpgradableCharacterLogic target)
        {
            base.ApplyUpgrade(target);
            NeroVisual visual = target.Body.Visual as NeroVisual;
            visual.EnableSword();
        }
    }
}
