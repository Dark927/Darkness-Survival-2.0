
using System;
using Characters.Common.Visual;
using Settings.Global;
using UnityEngine;

namespace Characters.Interfaces
{
    public interface IEntityBody : IEventsConfigurable, IResetable, IInitializable, IDisposable
    {
        public Transform Transform { get; }
        public IEntityView View { get; }
        public IEntityVisual Visual { get; }

        public bool IsReady { get; }
    }
}
