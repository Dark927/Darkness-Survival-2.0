using Characters.Enemy.Data;
using UnityEngine;

namespace World.Components
{
    public class EnemyContainer
    {
        #region Fields 

        public const string ContainerDefaultName = "Enemies_Container";
        private GameObjectsContainer _objectsContainer;

        #endregion


        #region Properties 

        public GameObjectsContainer ObjectsContainer => _objectsContainer;

        #endregion


        #region Methods 

        #region Init

        public EnemyContainer(GameObjectsContainer container, string preferedContainerName = ContainerDefaultName)
        {
            InitObjectsContainer(container, preferedContainerName);
        }

        private void InitObjectsContainer(GameObjectsContainer container, string preferedContainerName = ContainerDefaultName)
        {
            if (container != null)
            {
                _objectsContainer = container;

                if (preferedContainerName != ContainerDefaultName)
                {
                    _objectsContainer.name = preferedContainerName;
                }

                return;
            }

            GameObject obj = new GameObject(preferedContainerName, typeof(GameObjectsContainer));
            _objectsContainer = obj.GetComponent<GameObjectsContainer>();
        }

        #endregion

        public GameObjectsContainer GetChildContainer(EnemyData data, bool forceCreate = true)
        {
            string outerContainerName = ($"{data.Name} container").Replace(" ", "_");
            string innerContainerName = ($"{data.Type}").Replace(" ", "_");

            return _objectsContainer.GetChild(outerContainerName, forceCreate).GetChild(innerContainerName, forceCreate);
        }

        #endregion
    }
}
