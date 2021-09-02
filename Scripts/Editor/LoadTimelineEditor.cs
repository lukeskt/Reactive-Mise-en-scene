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

        //SerializedProperty presetTendency;
        //string[] editorTendency;
        //SerializedProperty tendencyIndex;
        
        SerializedProperty localeRequest;
        string[] editorLocale;        
        SerializedProperty localeIndex;

        SerializedProperty timelines;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            algorithm = serializedObject.FindProperty("algorithm");
            requestType = serializedObject.FindProperty("requestType");
            var loadTimeline = target as LoadTimeline;
            if (loadTimeline.RMSettings != null)
            {
                //editorTendency = loadTimeline.RMSettings.Tendencies;
                editorLocale = loadTimeline.RMSettings.Locales;
            }
            //presetTendency = serializedObject.FindProperty("presetTendency");
            //tendencyIndex = serializedObject.FindProperty("tendencyIndex");
            localeRequest = serializedObject.FindProperty("localeRequest");
            localeIndex = serializedObject.FindProperty("localeIndex");
            timelines = serializedObject.FindProperty("timelines");
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
                //editorTendency = loadTimeline.RMSettings.Tendencies;
                //presetTendency.stringValue = editorTendency[tendencyIndex.intValue];

                if (loadTimeline.algorithm != ReactiveMesSettings.SingleResultTendencyAlgorithm.Random)
                {
                    EditorGUILayout.PropertyField(requestType);
                    editorLocale = loadTimeline.RMSettings.Locales;
                    if (loadTimeline.requestType == ReactiveMesSettings.RequestType.Locale)
                    {
                        localeIndex.intValue = EditorGUILayout.Popup("Locale to Request:", localeIndex.intValue, editorLocale);
                    }
                    localeRequest.stringValue = editorLocale[localeIndex.intValue];
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(timelines);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif