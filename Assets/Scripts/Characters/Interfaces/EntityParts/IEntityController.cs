

using System;
using Characters.Common.Features;
using Characters.Common.Settings;
using Settings.Global;

namespace Characters.Common
{
    public interface IEntityController : IEventsConfigurable, IDisposable
    {
        public IEntityDynamicLogic EntityLogic { get; }
        public EntityCustomFeaturesHolder<IEntityFeature, IFeatureData> FeaturesHolder { get; }

        public void Initialize(IEntityData data);
        public void RemoveFeatures();
    }
}
