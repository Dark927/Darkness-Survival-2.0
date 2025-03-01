
using Characters.Common.Visual;
using Settings.Global;
using System;
using UnityEngine;

namespace Characters.Interfaces
{
    public interface IEntityBody : IEventListener, IResetable, IInitializable, IDisposable
    {
        public Transform Transform { get; }
        public IEntityView View { get; }
        public IEntityVisual Visual { get; }

        public bool IsReady { get; }
    }
}
