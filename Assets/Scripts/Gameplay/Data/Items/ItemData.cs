using UnityEngine;

namespace Gameplay.Components.Items
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "Game/Characters/Items/ItemData")]
    public class ItemData : ScriptableObject
    {
        #region Fields 

        [Header("Main - Settings")]
        [SerializeField] private GameObject _prefab;
        [SerializeField, TextArea(2, 5)] private string _description;

        [Space]
        [Header("Parameters - Settings")]
        [SerializeField] private ItemParametersBase _parameters;

        #endregion


        #region Properties

        public GameObject Prefab => _prefab;
        public string Description => _description;
        public virtual IItemParameters Parameters => _parameters;

        #endregion
    }
}