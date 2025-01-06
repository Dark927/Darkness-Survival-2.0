using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utilities.ErrorHandling;

namespace Utilities.Characters
{
    public static class CharacterSettingsValidator
    {
        public static void CheckCharacterBodyStatus(CharacterBody body)
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
