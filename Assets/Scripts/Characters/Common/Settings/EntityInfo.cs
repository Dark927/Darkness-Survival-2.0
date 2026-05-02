

using UnityEngine;

namespace Characters.Common.Settings
{
    public enum EntityType
    {
        Undefined,
        Player,
        Enemy,
    }

    [System.Serializable]
    public struct EntityInfo
    {
        [SerializeField] private EntityType _entityType;
        [SerializeField] private string _name;

        private int _typeID;

        public string Name => _name;
        public int TypeID => _typeID;
        public EntityType EntityType => _entityType;

        public void UpdateTypeID(int typeID)
        {
            _typeID = typeID;
        }
    }
}
