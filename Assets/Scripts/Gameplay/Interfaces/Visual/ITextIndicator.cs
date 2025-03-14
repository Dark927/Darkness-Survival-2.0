﻿
using System;
using Settings.Global;
using TMPro;

namespace Gameplay.Visual
{
    public interface ITextIndicator : IInitializable
    {
        public TMP_Text TMPText { get; }
        public event Action<ITextIndicator> OnLifeTimeEnd;
    }
}
