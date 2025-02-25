using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ReverseAnimation : EditorWindow
{
    [MenuItem("Tools/Invert Animation Clip")]
    public static void ShowWindow()
    {
        GetWindow<ReverseAnimation>("Invert Animation Clip");
    }

    private AnimationClip selectedClip;

    private void OnGUI()
    {
        GUILayout.Label("Invert Selected Animation Clip", EditorStyles.boldLabel);

        selectedClip = (AnimationClip)EditorGUILayout.ObjectField("Animation Clip", selectedClip, typeof(AnimationClip), false);

        if (selectedClip != null)
        {
            if (GUILayout.Button("Invert Clip"))
            {
                InvertAnimationClip(selectedClip);
            }
        }
    }

    private void InvertAnimationClip(AnimationClip clip)
    {
        if (clip == null) return;

        // Get all curve bindings in the animation clip
        var curveBindings = AnimationUtility.GetCurveBindings(clip);

        // Create a list to hold reversed keyframes
        List<AnimationCurve> reversedCurves = new List<AnimationCurve>();

        foreach (var binding in curveBindings)
        {
            // Get the original curve
            AnimationCurve curve = AnimationUtility.GetEditorCurve(clip, binding);

            // Reverse the keyframes in the curve
            AnimationCurve reversedCurve = ReverseCurve(curve, clip.length);

            // Add the reversed curve to the list
            reversedCurves.Add(reversedCurve);

            // Apply the reversed curve back to the clip
            AnimationUtility.SetEditorCurve(clip, binding, reversedCurve);
        }

        // Save changes
        EditorUtility.SetDirty(clip);
        AssetDatabase.SaveAssets();
    }

    private AnimationCurve ReverseCurve(AnimationCurve curve, float clipLength)
    {
        Keyframe[] keys = curve.keys;
        for (int i = 0; i < keys.Length; i++)
        {
            // Invert the time of each keyframe
            keys[i].time = clipLength - keys[i].time;
        }
        return new AnimationCurve(keys);
    }
}
