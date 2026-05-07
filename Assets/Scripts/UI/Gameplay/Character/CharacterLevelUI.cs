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

    [Tooltip("Time in seconds to fill the entire 100% of the level bar")]
    [SerializeField] private float _fullBarFillDuration = 1f;

    private PlayerCharacterController _targetCharacter;
    private TMP_Text _levelText;
    private SliderUI _levelXpSlider;

    private bool _canUpdateXpSlider = true;

    // Stores the target ratio to reach after the current level-up finishes
    private float _pendingTargetXpRatio = -1f;

    // Caches the current visual progress to calculate the lerp distance
    private float _currentVisualRatio = 0f;

    private bool _isLevelUpPlaying;
    private readonly Queue<CharacterLevelArgs> _levelUpQueue = new();

    #endregion

    #region Init

    [Inject]
    public void Construct(PlayerCharacterController characterController)
    {
        _targetCharacter = characterController;
    }

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

    #endregion

    private void LevelChangedListener(object sender, EntityLevelArgs args)
    {
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

        // Calculate duration to reach 100% from the current visual position
        float durationToFull = CalculateDurationToTarget(1f);

        _levelXpSlider.UpdatePercent(1f, durationToFull, false, callback: () =>
        {
            _levelText.text = characterArgs.ActualLevel.ToString();
            OnLevelNumberUpdate?.Invoke(this, characterArgs.ActualLevel);

            // Instantly reset the slider to 0
            _levelXpSlider.UpdatePercent(0f, 0f, false, callback: () =>
            {
                _currentVisualRatio = 0f;
                _isLevelUpPlaying = false;

                if (_levelUpQueue.Count > 0)
                {
                    TryPlayNextLevelUp();
                }
                else
                {
                    _canUpdateXpSlider = true;

                    // Animate to pending XP if any was collected during the level-up sequence
                    if (_pendingTargetXpRatio >= 0f)
                    {
                        AnimateToRatio(_pendingTargetXpRatio);
                        _pendingTargetXpRatio = -1f;
                    }
                }
            });
        }, true);
    }

    private void XpUpdatedListener(object sender, CharacterLevelArgs args)
    {
        if (_canUpdateXpSlider)
        {
            AnimateToRatio(args.XpProgressRatio);
        }
        else
        {
            // Overwrite the final target ratio to avoid queueing multiple micro-animations
            _pendingTargetXpRatio = args.XpProgressRatio;
        }
    }

    private void AnimateToRatio(float targetRatio)
    {
        float duration = CalculateDurationToTarget(targetRatio);
        _levelXpSlider.UpdatePercent(targetRatio, duration, false);
        _currentVisualRatio = targetRatio;
    }

    // Calculates dynamic duration based on the actual UI distance the slider needs to travel
    private float CalculateDurationToTarget(float targetRatio)
    {
        float distanceToTravel = Mathf.Abs(targetRatio - _currentVisualRatio);

        // Minimum clamp (e.g. 0.05s) prevents instantaneous/zero-duration tweens
        return Mathf.Max(0.05f, distanceToTravel * _fullBarFillDuration);
    }
}
