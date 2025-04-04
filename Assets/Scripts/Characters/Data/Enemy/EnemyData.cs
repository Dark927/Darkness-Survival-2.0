﻿using System;
using System.Collections.Generic;
using Characters.Common.Settings;
using Gameplay.Components.Items;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Characters.Enemy.Settings
{
    public enum EnemyType
    {
        Default = 0,
        Toxin = 1,
        Bloody = 2,
        Golden = 3,
    }

    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/Characters/Enemy/EnemyData")]
    public class EnemyData : AttackableCharacterData
    {
        #region Fields 


        [Header("Custom Enemy Settings")]

        [SerializeField] private EnemyType _type;

        [Header("Drop Items Settings")]
        [SerializeField] private List<AssetReferenceT<ItemDropData>> _dropItemReferences;

        #endregion


        #region Properties

        public EnemyType Type => _type;
        public List<AssetReferenceT<ItemDropData>> DropItemReferences => _dropItemReferences;


        #endregion


        #region Methods

        public override int GetHashCode()
        {
            int hash = HashCode.Combine(
                CommonInfo.Name != null ? CommonInfo.Name.GetHashCode() : 0,
                Type.GetHashCode(),
                Stats.GetHashCode()
            );
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj is not EnemyData other)
                return false;

            return CommonInfo.Name == other.CommonInfo.Name &&
                   Type == other.Type &&
                   Stats.Equals(other.Stats);
        }

        #endregion
    }
}
