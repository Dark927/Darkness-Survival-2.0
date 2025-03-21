﻿
using System;
using Characters.Interfaces;
using UnityEngine;

namespace Characters.Common.Features
{
    public interface IEntityFeature : IDisposable
    {
        public enum TargetEntityPart
        {
            Base = 0,
            Body,
        }

        public bool IsReady { get; }
        public void Initialize(IEntityDynamicLogic characterLogic);
        public TargetEntityPart EntityConnectionPart { get; }
        public GameObject RootObject { get; }
    }
}
