using System;
using System.Collections.Generic;
using System.Linq;
using Settings.Global;
using UnityEngine;


namespace Settings.Abstract
{
    public class LazySingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static Lazy<T> _lazyInstance = new Lazy<T>(() =>
        {
            T concreteInstance = FindObjectOfType<T>();

            if (concreteInstance == null)
            {
                GameObject singletonObject = new GameObject(typeof(T).Name);
                concreteInstance = singletonObject.AddComponent<T>();

                if (concreteInstance is IInitializable initializableInstance)
                {
                    initializableInstance.Initialize();
                }
            }
            return concreteInstance;
        });

        public static T Instance => _lazyInstance.Value;
    }
}
