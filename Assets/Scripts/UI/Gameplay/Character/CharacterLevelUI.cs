using System;
using System.Collections.Generic;
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

    [SerializeField] private float _levelRaiseTimeMult = 1f;

    private PlayerCharacterController _targetCharacter;
    private TMP_Text _levelText;
    private SliderUI _levelXpSlider;

    private bool _canUpdateXpSlider = true;
    private Queue<Action> _xpUpdateQueue = new();

    private bool _isLevelUpPlaying;
    private readonly Queue<CharacterLevelArgs> _levelUpQueue = new();

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
        _levelUpQueue.Enqueue((CharacterLevelArgs)args);
        TryPlayNextLevelUp();
    }

    private void TryPlayNextLevelUp()
    {
        if (_isLevelUpPlaying || _levelUpQueue.Count == 0)
        {
            return;
        }

        _isLevelUpPlaying = true;
        _canUpdateXpSlider = false;

        var characterArgs = _levelUpQueue.Dequeue();
        var duration = CalculateXpLerpDuration(characterArgs.ActualXpBounds);

        _levelXpSlider.UpdatePercent(1f, duration, false, callback: () =>
        {
            _levelText.text = characterArgs.ActualLevel.ToString();
            OnLevelNumberUpdate?.Invoke(this, characterArgs.ActualLevel);

            _levelXpSlider.UpdatePercent(0f, 0f, false, callback: () =>
            {
                _isLevelUpPlaying = false;

                if (_levelUpQueue.Count == 0)
                {
                    FlushXpQueue();
                    _canUpdateXpSlider = true;
                }

                TryPlayNextLevelUp();
            });
        }, true);
    }

    private void XpUpdatedListener(object sender, CharacterLevelArgs args)
    {
        float duration = CalculateXpLerpDuration(args.ActualXpBounds);

        if (_canUpdateXpSlider)
        {
            _levelXpSlider.UpdatePercent(args.XpProgressRatio, duration, false);
        }
        else
        {
            _xpUpdateQueue.Enqueue(() => _levelXpSlider.UpdatePercent(args.XpProgressRatio, duration, false));
        }
    }

    private void FlushXpQueue()
    {
        while (_xpUpdateQueue.Count > 0)
        {
            _xpUpdateQueue.Dequeue().Invoke();
        }
    }

    private float CalculateXpLerpDuration((float previous, float next) actualXpBounds)
    {
        var (previousXpBound, nextXpBound) = actualXpBounds;
        return 1f * (_levelRaiseTimeMult) / ((nextXpBound / previousXpBound));
    }

    #endregion
}
