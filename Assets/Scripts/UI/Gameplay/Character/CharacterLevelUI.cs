using Characters.Common.Levels;
using Characters.Player;
using TMPro;
using UnityEngine;
using Zenject;

public class CharacterLevelUI : MonoBehaviour
{
    #region Fields 

    [SerializeField] private float _levelDropTimeSec = 0.1f;
    [SerializeField] private float _levelRaiseTimeMult = 1f;

    private PlayerCharacterController _targetCharacter;
    private TMP_Text _levelText;
    private SliderUI _levelXpSlider;

    #endregion


    #region Properties

    #endregion


    #region Methods

    #region Init

    [Inject]
    public void Construct(PlayerCharacterController characterController)
    {
        _targetCharacter = characterController;
    }


    #endregion

    private void Awake()
    {
        _levelText = GetComponentInChildren<TMP_Text>();
        _levelXpSlider = GetComponentInChildren<SliderUI>();

        _targetCharacter.CharacterLogic.Level.OnLevelUp += LevelChangedListener;
        _targetCharacter.CharacterLogic.Level.OnUpdateXp += XpUpdatedListener;
    }

    private void OnDestroy()
    {
        _targetCharacter.CharacterLogic.Level.OnLevelUp -= LevelChangedListener;
        _targetCharacter.CharacterLogic.Level.OnUpdateXp -= XpUpdatedListener;
    }


    private void LevelChangedListener(IEntityLevel entityLevel)
    {
        ICharacterLevel characterLevel = entityLevel as ICharacterLevel;
        float duration = CalculateXpLerpDuration(characterLevel, characterLevel.ActualXpBounds.Item1);

        _levelXpSlider.UpdatePercent(1f, duration, callback: () => _levelText.text = characterLevel.ActualLevel.ToString());
        _levelXpSlider.UpdatePercent(0f, _levelDropTimeSec);
    }

    private void XpUpdatedListener(ICharacterLevel characterLevel)
    {
        float duration = CalculateXpLerpDuration(characterLevel, characterLevel.ActualXpBounds.Item2);
        _levelXpSlider.UpdatePercent(characterLevel.XpProgressRatio, duration);
    }

    private float CalculateXpLerpDuration(ICharacterLevel characterLevel, float levelTargetXp)
    {
        var bounds = characterLevel.ActualXpBounds;
        return 1f * (_levelRaiseTimeMult) / ((bounds.Item2 / bounds.Item1));
    }


    #endregion
}
