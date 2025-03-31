using System;
using UnityEngine;

namespace Utilities.ErrorHandling //TODO:
{
    public static class ErrorLogger
    {
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogError(string message)
        {
            Debug.LogError(message);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        // https://nosuchstudio.medium.com/improve-unitys-logging-with-this-class-fc99c91f5564
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogComponentIsNull(string objectName, string componentName)
        {
            Debug.LogError($"{objectName} has {componentName} component == null");
        }
    }
}
