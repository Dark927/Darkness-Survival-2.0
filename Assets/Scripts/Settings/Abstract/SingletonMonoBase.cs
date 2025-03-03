using UnityEngine;

namespace Settings.Abstract
{
    public abstract class SingletonMonoBase<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T _instance;
        public static T Instance => _instance;

        protected virtual void InitInstance(bool isPersistent = false)
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this as T;

            if (isPersistent)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
