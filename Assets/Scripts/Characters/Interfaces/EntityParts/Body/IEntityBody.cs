
using System;
using Characters.Common.Visual;
using Settings.Global;
using UnityEngine;

namespace Characters.Common
{
    public interface IEntityBody : IEventsConfigurable, IResetable, IInitializable, IDisposable
    {
        public Transform OriginalTransform { get; }
        public Transform TargetingTransform { get; }
        public IEntityView View { get; }
        public IEntityVisual Visual { get; }

        public bool IsReady { get; }
    }
}
