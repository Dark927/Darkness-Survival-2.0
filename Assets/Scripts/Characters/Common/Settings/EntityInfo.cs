

using UnityEngine;

namespace Characters.Common.Settings
{
    [System.Serializable]
    public struct EntityInfo
    {
        [SerializeField] private string _name;

        private int _typeID;

        public string Name => _name;
        public int TypeID => _typeID;

        public void UpdateTypeID(int typeID)
        {
            _typeID = typeID;
        }
    }
}
