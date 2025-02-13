using Characters.Common;
using Utilities.ErrorHandling;

namespace Utilities.Characters
{
    public static class CharacterSettingsValidator
    {
        public static void CheckCharacterBodyStatus(EntityBodyBase body)
        {
            if (body.View == null)
            {
                ErrorLogger.LogComponentIsNull(LogOutputType.Console, body.name, nameof(body.View));
            }

            if (body.Movement == null)
            {
                ErrorLogger.LogComponentIsNull(LogOutputType.Console, body.name, nameof(body.Movement));
            }

            if (body.Visual == null)
            {
                ErrorLogger.LogComponentIsNull(LogOutputType.Console, body.name, nameof(body.Visual));
            }
        }
    }
}
