
using System;
using Settings.Global;
using TMPro;
using UnityEngine;

namespace Gameplay.Visual
{
    public interface ITextIndicator : IInitializable
    {
        public GameObject gameObject { get; }
        public TMP_Text TMPText { get; }
        public event Action<ITextIndicator> OnLifeTimeEnd;
    }
}
