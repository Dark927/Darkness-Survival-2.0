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

        private float _actualXp = 1f;
        private float _gainedXpFactor;
        private CharacterLevelData _levelData;

        #endregion


        #region Properties

        public float ActualXp => _actualXp;
        public override int ActualLevel => 1 + (int)LevelFunctionInv(_actualXp);
        public float XpProgressRatio => InterpolateRealXp(_actualXp, _actualXp);

        public (float, float) ActualXpBounds => GetRealLevelBounds(_actualXp);
        public float GainedXpFactor => _gainedXpFactor;

        #endregion


        #region Events

        public event Action<ICharacterLevel> OnUpdateXp;

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
            _gainedXpFactor = InterpolateRealXp(_actualXp, _actualXp + xp);

            _actualXp += xp;

            while (_gainedXpFactor >= 1.0f)
            {
                LevelUp();
                _gainedXpFactor -= 1;
            }

            OnUpdateXp?.Invoke(this);
        }


        #endregion

        #region Level Calculation

        //get full levels between current
        public (float, float) GetRealLevelBounds(float currentLevel)
        {
            var (prevLvl, nextLvl) = GetIncrementalLevelBounds(currentLevel);
            return (LevelFunction(prevLvl), LevelFunction(nextLvl));
        }

        private float LevelFunction(float x)
        {
            return _levelData.Multiplier * Mathf.Pow(x, _levelData.Exp) + _levelData.Starter;
        }

        private float LevelFunctionInv(float y)
        {
            return Mathf.Pow((y - _levelData.Starter) / _levelData.Multiplier, 1 / _levelData.Exp);
        }

        //get integer levels between current
        private (int, int) GetIncrementalLevelBounds(float currentLevel)
        {
            float levelNumberRational = LevelFunctionInv(currentLevel);
            return ((int)Mathf.Floor(levelNumberRational), (int)Mathf.Ceil(levelNumberRational));
        }

        private float InterpolateRealXp(float currentLevel, float nextLevel)
        {
            var (prevLvl, nextLvl) = GetRealLevelBounds(currentLevel);
            return CustomMath.InverseLerpUnclamped(prevLvl, nextLvl, nextLevel);
        }

        #endregion

    }
}