#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(LoadTimeline))]
    [CanEditMultipleObjects]
    public class LoadTimelineEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty algorithm;
        SerializedProperty requestType;

        SerializedProperty presetTendency;
        string[] editorTendency;
        SerializedProperty tendencyIndex;
        
        SerializedProperty localeRequest;
        string[] editorLocale;        
        SerializedProperty localeIndex;

        SerializedProperty timelines;
        SerializedProperty presetTimeline;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            algorithm = serializedObject.FindProperty("algorithm");
            var loadTimeline = target as LoadTimeline;
            if (loadTimeline.RMSettings != null)
            {
                editorTendency = loadTimeline.RMSettings.Tendencies;
                editorLocale = loadTimeline.RMSettings.Locales;
            }
            requestType = serializedObject.FindProperty("requestType");
            presetTendency = serializedObject.FindProperty("presetTendency");
            tendencyIndex = serializedObject.FindProperty("tendencyIndex");
            localeRequest = serializedObject.FindProperty("localeRequest");
            localeIndex = serializedObject.FindProperty("localeIndex");
            timelines = serializedObject.FindProperty("timelines");
            presetTimeline = serializedObject.FindProperty("presetTimeline");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            EditorGUILayout.PropertyField(algorithm);
            var loadTimeline = target as LoadTimeline;
            if (loadTimeline.RMSettings != null)
            {
                editorTendency = loadTimeline.RMSettings.Tendencies;
                if (loadTimeline.algorithm == ReactiveMesSettings.TendencyAlgorithm.Preset) // preset algo - index not ideal, name match how?
                {
                    tendencyIndex.intValue = EditorGUILayout.Popup("Preset Tendency:", tendencyIndex.intValue, editorTendency);
                }
                presetTendency.stringValue = editorTendency[tendencyIndex.intValue];

                EditorGUILayout.PropertyField(requestType);

                editorLocale = loadTimeline.RMSettings.Locales;
                if (loadTimeline.requestType == ReactiveMesSettings.RequestType.Locale)
                {
                    localeIndex.intValue = EditorGUILayout.Popup("Locale to Request:", localeIndex.intValue, editorLocale);
                }
                localeRequest.stringValue = editorLocale[localeIndex.intValue];
            }
            if (loadTimeline.algorithm == ReactiveMesSettings.TendencyAlgorithm.Preset)
            {
                EditorGUILayout.PropertyField(presetTimeline);
            }
            else
            {
                EditorGUILayout.PropertyField(timelines);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif