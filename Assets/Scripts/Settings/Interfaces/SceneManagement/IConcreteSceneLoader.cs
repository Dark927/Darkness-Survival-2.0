using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Settings.SceneManagement
{
    public interface IConcreteSceneLoader<in TLoadParameter, out TLoadReturnParameter>
    {
        public IEnumerable<TLoadReturnParameter> LoadMultipleScenes(IEnumerable<TLoadParameter> loadParameters);
        public TLoadReturnParameter LoadScene(TLoadParameter parameter, LoadSceneMode loadMode = LoadSceneMode.Single);
        public void UnloadAll();
    }
}
