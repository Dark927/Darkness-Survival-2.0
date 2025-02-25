
using UnityEngine;

namespace Gameplay.Components.Items
{
    [CreateAssetMenu(fileName = "NewItemXpData", menuName = "Game/Characters/ItemsItemXpData")]
    public class ItemXpData : ItemData
    {
        [SerializeField] private ItemXpParameters _xpParameters;

        public override IItemParameters Parameters => _xpParameters;
    }
}
