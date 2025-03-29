using Characters.Interfaces;
using Gameplay.Components.Items;
using UnityEngine;

public class HealCapsule : PickupItemBase
{

    #region Fields 

    private ItemHpParameters _itemHpParameters = new ItemHpParameters();

    #endregion


    #region Properties

    public override IItemParameters Parameters => _itemHpParameters;

    #endregion


    #region Methods

    #region Init

    #endregion

    public override void Pickup(ICharacterLogic targetCharacter)
    {
        if (_itemHpParameters.HpAmount <= 0)
        {
            return;
        }

        if (_itemHpParameters.PermanentRegeneration)
        {
            ApplyPermanentRegeneration(targetCharacter, _itemHpParameters);
        }
        else
        {
            ApplyTemporaryRegeneration(targetCharacter, _itemHpParameters);
        }
    }

    private void ApplyTemporaryRegeneration(ICharacterLogic targetCharacter, ItemHpParameters parameters)
    {
        float stepHpAmount = Mathf.Approximately(parameters.DurationInSec, 0f)
                            ? parameters.HpAmount
                            : Mathf.RoundToInt(parameters.HpAmount / parameters.DurationInSec);

        if (Mathf.Approximately(stepHpAmount, 0f))
        {
            return;
        }

        int stepCount = (int)(parameters.HpAmount / stepHpAmount);
        float stepInSec = parameters.DurationInSec / stepCount;

        targetCharacter.Body.Health.RegenerateHpDuringTime(stepHpAmount, parameters.DurationInSec, stepInSec, true);
    }

    private void ApplyPermanentRegeneration(ICharacterLogic targetCharacter, ItemHpParameters parameters)
    {
        targetCharacter.Body.Health.RegenerateHpAlways(parameters.HpAmount, additive: true);
    }

    #endregion

}
