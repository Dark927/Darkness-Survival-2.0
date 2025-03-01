namespace Extra
{
    internal class ExtraCode
    {


        // From PlayerCharacterLogic -> AddXp()

        public void DisplayLevelSettings()
        {
            //#if UNITY_EDITOR
            //        UnityEngine.Debug.Log($"" +
            //                $"<color=blue> xp: {xp}  </color>  |   " +
            //                $"<color=green> _actualXp:{_actualXp} </color>    |    " +
            //                $"XpProgressRatio:{XpProgressRatio}  |     " +
            //                $"t:{GainedXpFactor}     |  <color=yellow>  " +
            //                $"ActualLevel: {ActualLevel} </color>      |  " +
            //                $"XpBottomBount: {XpBounds.Item1}     " +
            //                $"XpToNextLim:{XpBounds.Item2}");
            //#endif

        }

        public void DisplayHpSettings()
        {

            //[Header("<color=yellow>The character can regenerate the whole HP number only," +
            //    "\nso very small/float values will be rounded" +
            //    "\nFor example, if duration == 100 & HP Amount == 1," +
            //    "\nthe character will regen 1 hp/sec. (not 0.01), etc.</color>")]
            //UnityEngine.Debug.Log($"step hp amount : {stepHpAmount}           |    step count : {stepCount}         |     step in sec : {stepInSec}");
        }
    }
}
