using System.Collections.Generic;
using Characters.Common.Features;
using Cysharp.Threading.Tasks;

namespace Characters.Common.Abilities
{
    public class EntityPassiveAbilitiesHandler : IFeaturesHolderProvider<IEntityAbility>
    {
        #region Events

        #endregion

        #region Fields


        private EntityCustomFeaturesHolder<IEntityAbility, IAbilityData> _abilitiesHolder;
        private IEntityDynamicLogic _ownerLogic;

        #endregion


        #region Properties

        public IEnumerable<IEntityAbility> ActiveOnes => _abilitiesHolder.ActiveOnesDict.Values;
        public string DefaultContainerName => $"{_ownerLogic.Info.Name ?? "character"}_abilities";

        #endregion


        #region Methods 

        #region Init

        public EntityPassiveAbilitiesHandler(IEntityDynamicLogic owner, string abilitiesContainerName = null)
        {
            _ownerLogic = owner;
            abilitiesContainerName ??= DefaultContainerName;
            _abilitiesHolder = new EntityCustomFeaturesHolder<IEntityAbility, IAbilityData>(owner, abilitiesContainerName);
            _abilitiesHolder.Initialize();
        }


        public void Dispose()
        {
            if (_abilitiesHolder == null)
            {
                return;
            }

            _abilitiesHolder.Dispose();
        }

        #endregion

        public bool TryGetFeatureByID(int abilityID, out IEntityAbility ability)
        {
            ability = null;
            return (_abilitiesHolder != null) && _abilitiesHolder.ActiveOnesDict.TryGetValue(abilityID, out ability);
        }

        public UniTask GiveAbilityAsync(IAbilityData abilityData)
        {
            return _abilitiesHolder.GiveAsync(abilityData);
        }

        #endregion
    }
}
