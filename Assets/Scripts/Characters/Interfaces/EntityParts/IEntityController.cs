

using System;
using Characters.Common.Features;
using Characters.Interfaces;
using Characters.Stats;
using Settings.Global;

namespace Characters.Common
{
    public interface IEntityController : IEventsConfigurable, IDisposable
    {
        public IEntityDynamicLogic EntityLogic { get; }
        public EntityCustomFeaturesHolder FeaturesHolder { get; }

        public void Initialize(IEntityData data);
        public void RemoveFeatures();
    }
}
