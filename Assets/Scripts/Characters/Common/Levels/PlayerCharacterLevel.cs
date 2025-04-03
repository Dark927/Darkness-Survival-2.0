using System;
using Characters.Common.Levels;
using Characters.Player.Data;
using UnityEngine;
using Utilities.Math;


namespace Characters.Player.Levels
{
    public class PlayerCharacterLevel : EntityLevelBase, ICharacterLevel
    {
        #region Fields

        private float _actualXp = 0f;
        private float _gainedXpFactor = 0f;
        private CharacterLevelData _levelData;

        #endregion


        #region Properties

        public float ActualXp => _actualXp;
        public override int ActualLevel => 1 + (int)LevelFunctionInv(_actualXp); //start from 1
        public float XpProgressRatio => InterpolateRatio(ActualLevel, _actualXp);

        public (float previous, float next) ActualXpBounds => LevelToXpBounds(ActualLevel);
        public float GainedXpFactor => _gainedXpFactor;

        #endregion


        #region Events

        public event EventHandler<CharacterLevelArgs> OnUpdateXp;

        #endregion


        #region Methods

        #region Init

        public PlayerCharacterLevel(CharacterLevelData characterLevelData)
        {
            _levelData = characterLevelData;
        }

        #endregion


        public void AddXp(int xp)
        {
            //remember data
            float oldXp = _actualXp;
            int levelProgress = ActualLevel;
            (float previous, float next) levelProgressBounds = LevelToXpBounds(levelProgress);

            // add xp to player
            _actualXp += xp;

            while (_actualXp >= levelProgressBounds.next) //our level is bigger than bound
            {
                levelProgress++;
                levelProgressBounds = LevelToXpBounds(levelProgress);
                //Debug.Log($"Sent animation, lvl: {levelProgress}, bounds: [{levelProgressBounds.previous} - {levelProgressBounds.next})");

                LevelUp(new CharacterLevelArgs(levelProgress, levelProgressBounds, 0.0f));
            }

            OnUpdateXp?.Invoke(this, new CharacterLevelArgs(ActualLevel, ActualXpBounds, XpProgressRatio));
            //Debug.Log($"Current XP: {ActualXp}, level: {ActualLevel}");
        }


        #endregion

        #region Level Calculation

        //get full levels between current
        // 0 -> Undefined
        // 1 -> [0,20)
        // 2 -> [20,91)
        // 3 -> [90,224)
        public (float, float) LevelToXpBounds(int currentLevel)
        {
            return (LevelFunction(currentLevel - 1), LevelFunction(currentLevel));
        }

        // level starts from zero
        private float LevelFunction(float x)
        {
            return _levelData.Multiplier * Mathf.Pow(x, _levelData.Exp) + _levelData.Starter;
        }
        // level starts from zero
        private float LevelFunctionInv(float y)
        {
            return Mathf.Pow((y - _levelData.Starter) / _levelData.Multiplier, 1 / _levelData.Exp);
        }

        //get a percentage on xp bar.
        // for lvl2, xp = 50, bounds are [20,91) and progress: 42%
        private float InterpolateRatio(int currentLevel, float currentXp)
        {
            var bounds = LevelToXpBounds(currentLevel);
            return CustomMath.InverseLerpUnclamped(bounds.Item1, bounds.Item2, currentXp);
        }

        #endregion

    }
}
