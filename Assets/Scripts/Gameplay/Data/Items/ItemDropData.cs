using System;
using UnityEngine;

namespace Gameplay.Components.Items
{
    [CreateAssetMenu(fileName = "NewItemDropData", menuName = "Game/Characters/Items/ItemDropData")]
    public class ItemDropData : ScriptableObject
    {
        [SerializeField] private ItemDropSettings _dropSettings;

        public ItemDropSettings DropSettings => _dropSettings;
    }
}
