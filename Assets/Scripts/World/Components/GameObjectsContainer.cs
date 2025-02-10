using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace World.Components
{
    public class GameObjectsContainer : MonoBehaviour
    {
        #region Fields 

        private List<GameObjectsContainer> _childContainers;

        #endregion


        #region Methods 

        #region Init

        private void Awake()
        {
            _childContainers = new List<GameObjectsContainer>();
        }

        #endregion

        public GameObjectsContainer GetChild(string name, bool enableForceCreate = false)
        {
            if (!HasChildInList(name) && enableForceCreate)
            {
                AddChild(name);
            }

            return _childContainers.FirstOrDefault(child => child.name == name);
        }

        public bool HasChild(string targetName, bool searchInChilds = false)
        {
            if ((_childContainers == null) || (_childContainers.Count == 0))
            {
                return false;
            }

            if (HasChildInList(targetName))
            {
                return true;
            }

            if (!searchInChilds)
            {
                return false;
            }


            bool childSearchResult = false;

            foreach (var child in _childContainers)
            {
                if (child.HasChild(targetName, searchInChilds))
                {
                    childSearchResult = true;
                    break;
                }
            }

            return childSearchResult;
        }

        public void AddChild(string name)
        {
            if (HasChildInList(name))
            {
                return;
            }

            GameObject childObj = new GameObject(name, typeof(GameObjectsContainer));
            childObj.transform.parent = transform;
            GameObjectsContainer objectsContainer = childObj.GetComponent<GameObjectsContainer>();

            _childContainers.Add(objectsContainer);
        }

        public GameObjectsContainer FindChild(string targetName, bool searchInChilds)
        {
            GameObjectsContainer container = FindChildInList(targetName);

            if (container != null)
            {
                return container;
            }

            if (!searchInChilds)
            {
                return null;
            }


            foreach (var child in _childContainers)
            {
                container = child.FindChild(targetName, searchInChilds);

                if (!ReferenceEquals(container, null))
                {
                    break;
                }
            }

            return container;
        }

        public void RemoveChild(string name, bool searchInChilds = false)
        {
            GameObjectsContainer child = FindChild(name, searchInChilds);

            if (ReferenceEquals(child, null))
            {
                return;
            }

            child.RemoveAllChilds();

            Destroy(child.gameObject);
        }

        public void RemoveAllChilds()
        {
            foreach (var child in _childContainers)
            {
                child.RemoveAllChilds();
            }

            for (int currentIndex = 0; currentIndex < _childContainers.Count; ++currentIndex)
            {
                Destroy(_childContainers[currentIndex].gameObject);
                _childContainers[currentIndex] = null;
            }

            _childContainers = null;
        }


        private bool HasChildInList(string targetName)
        {
            return _childContainers.Any(child => child.name == targetName);
        }

        private GameObjectsContainer FindChildInList(string targetName)
        {
            return _childContainers.FirstOrDefault(child => child.name == targetName);
        }

        #endregion
    }
}
