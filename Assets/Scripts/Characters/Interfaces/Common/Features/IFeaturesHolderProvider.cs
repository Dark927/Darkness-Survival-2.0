using System;

namespace Characters.Common.Features
{
    public interface IFeaturesHolderProvider<TTarget> : IDisposable
    {
        public bool TryGetFeatureByID(int featureID, out TTarget feature);
    }
}
