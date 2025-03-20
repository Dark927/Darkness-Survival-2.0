using System;
using UnityEngine;

namespace Utilities.ErrorHandling
{
    public enum LogOutputType
    {
        Console,
        txtFile,
    }

    public static class ErrorLogger
    {
        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        [System.Diagnostics.Conditional("ENABLE_LOG")]
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void LogComponentIsNull(LogOutputType type, string objectName, string componentName)
        {
            switch (type)
            {
                case LogOutputType.Console:
                    Debug.LogError($"{objectName} has {componentName} component == null");
                    break;

                default:
                    throw new NotImplementedException();
            }

        }
    }
}
