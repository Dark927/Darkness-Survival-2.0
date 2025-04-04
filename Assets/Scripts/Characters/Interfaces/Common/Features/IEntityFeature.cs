
using System;
using UnityEngine;

namespace Characters.Common.Features
{
    public interface IEntityFeature : IDisposable
    {
        public enum TargetEntityPart
        {
            UseContainerSettings = 0,
            Base,
            Body,
        }

        public void Initialize(IEntityDynamicLogic characterLogic);
        public TargetEntityPart EntityConnectionPart { get; }
        public GameObject RootObject { get; }
    }
}
