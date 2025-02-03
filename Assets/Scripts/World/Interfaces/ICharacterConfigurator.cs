using UnityEngine;

namespace World.Components
{
    public interface ICharacterConfigurator
    {
        public void Configure(GameObject characterObj, Transform targetTransform = null);
    }
}