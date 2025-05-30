﻿using System;
using UnityEngine;

namespace Gameplay.Components.Items
{
    [System.Serializable]
    public class ItemDropSettings
    {
        #region Fields 

        [SerializeField] private ItemData _itemData;
        [SerializeField, Range(0, 100)] private int _dropChance = 50;

        #endregion

        #region Properties

        public ItemData Data => _itemData;
        public int DropChance => _dropChance;

        #endregion
    }
}
