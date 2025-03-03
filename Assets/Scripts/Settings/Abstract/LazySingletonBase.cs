using Settings.Global;
using System;
using UnityEngine;


namespace Settings.Abstract
{
    public class LazySingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static Lazy<T> _lazyInstance = new Lazy<T>(() =>
        {
            T instance = FindObjectOfType<T>();

            if (instance == null)
            {
                GameObject singletonObject = new GameObject(typeof(T).Name);
                instance = singletonObject.AddComponent<T>();
            }
            return instance;
        });

        public static T Instance => _lazyInstance.Value;
    }
}
