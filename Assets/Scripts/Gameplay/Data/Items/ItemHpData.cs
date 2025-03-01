
using UnityEngine;

namespace Gameplay.Components.Items
{
    [CreateAssetMenu(fileName = "NewItemHpData", menuName = "Game/Characters/Items/ItemHpData")]
    public class ItemHpData : ItemData
    {
        [SerializeField] private ItemHpParameters _hpParameters;

        public override IItemParameters Parameters => _hpParameters;
    }
}
