using System;
using Characters.Common.Levels;
using Characters.Player;
using Characters.Player.Levels;
using TMPro;
using UI;
using UnityEngine;
using Zenject;

public class CharacterLevelUI : MonoBehaviour
{
    #region Events

    public event EventHandler<int> OnLevelNumberUpdate;

    #endregion


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


    private void LevelChangedListener(object sender, EntityLevelArgs args)
    {
        var characterArgs = args as CharacterLevelArgs;
        float duration = CalculateXpLerpDuration(characterArgs);

        _levelXpSlider.UpdatePercent(1f, duration, false, callback: () =>
        {
            _levelText.text = args.ActualLevel.ToString();
            OnLevelNumberUpdate?.Invoke(this, args.ActualLevel);
        }, true);
        _levelXpSlider.UpdatePercent(0f, _levelDropTimeSec, false);
    }

    private void XpUpdatedListener(object sender, CharacterLevelArgs args)
    {
        float duration = CalculateXpLerpDuration(args);
        _levelXpSlider.UpdatePercent(args.XpProgressRatio, duration, false);
    }

    private float CalculateXpLerpDuration(CharacterLevelArgs args)
    {
        var (previousXpBound, nextXpBound) = args.ActualXpBounds;
        return 1f * (_levelRaiseTimeMult) / ((nextXpBound / previousXpBound));
    }


    #endregion
}
