

using System;
using UnityEngine;

namespace Characters.Common.Visual
{
    public interface IEntityCustomVisualPart : IDisposable
    {
        public GameObject gameObject { get; }
        public void Initialize(IEntityBody targetEntityBody);
    }
}
