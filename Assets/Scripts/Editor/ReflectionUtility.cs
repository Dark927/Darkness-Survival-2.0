using System;
using System.Reflection;
using Dark.Utils;
using UnityEditor;
using UnityEngine;

namespace Dark.EditorUtils
{
    /// <summary>
    /// Helper class for reflection-based access to serialized property targets.
    /// </summary>
    public static class ReflectionUtility
    {
        /// <summary>
        /// Gets the target object of a SerializedProperty.
        /// </summary>
        /// <param name="prop">SerializedProperty to scan</param>
        /// <returns>object inside of a SerializedProperty or null</returns>
        /// <exception cref="NotImplementedException">If the object isn't directly in SerializedProperty</exception>
        public static object GetTargetObjectOfProperty(SerializedProperty prop)
        {
            if (prop == null) return null;

            var path = prop.propertyPath;
            object obj = prop.serializedObject.targetObject;
            if (path.Contains("."))
            {
                throw new NotImplementedException();
            }
            return GetValue(obj, path);
        }

        /// <summary>
        /// Get value of an object by name. (gets source."name")
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="name">field or property name</param>
        /// <returns>object with the value or null</returns>
        private static object GetValue(object source, string name)
        {
            if (source == null) return null;
            var type = source.GetType();
            var field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field == null)
            {
                var prop = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
                if (prop == null) return null;
                return prop.GetValue(source, null);
            }
            return field.GetValue(source);
        }
    }
}
