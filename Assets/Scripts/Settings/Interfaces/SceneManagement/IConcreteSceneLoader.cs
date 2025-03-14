using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Settings.SceneManagement
{
    public interface IConcreteSceneLoader<in TLoadParameter, out TLoadReturnParameter>
    {
        public IEnumerable<TLoadReturnParameter> LoadMultipleScenes(IEnumerable<TLoadParameter> loadParameters, bool activateOnLoad = false);
        public TLoadReturnParameter LoadScene(TLoadParameter parameter, LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true);
        public void UnloadAll();
    }
}
