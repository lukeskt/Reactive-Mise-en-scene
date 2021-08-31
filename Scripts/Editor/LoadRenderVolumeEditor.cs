using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(LoadRenderVolume))]
    [CanEditMultipleObjects]
    public class LoadRenderVolumeEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty algorithm;
        SerializedProperty requestType;
        SerializedProperty presetTendency;
        string[] editorTendency;
        SerializedProperty tendencyIndex;
        string[] editorLocale;
        SerializedProperty localeRequest;
        SerializedProperty localeIndex;
        SerializedProperty volumeProfiles;
        SerializedProperty presetVolumeProfile;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            algorithm = serializedObject.FindProperty("algorithm");
            var loadRenderVolume = target as LoadRenderVolume;
            if (loadRenderVolume.RMSettings != null)
            {
                editorTendency = loadRenderVolume.RMSettings.Tendencies;
                editorLocale = loadRenderVolume.RMSettings.Locales;
            }
            requestType = serializedObject.FindProperty("requestType");
            presetTendency = serializedObject.FindProperty("presetTendency");
            tendencyIndex = serializedObject.FindProperty("tendencyIndex");
            localeRequest = serializedObject.FindProperty("localeRequest");
            localeIndex = serializedObject.FindProperty("localeIndex");
            volumeProfiles = serializedObject.FindProperty("volumeProfiles");
            presetVolumeProfile = serializedObject.FindProperty("presetVolumeProfile");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            EditorGUILayout.PropertyField(algorithm);
            var loadRenderVolume = target as LoadRenderVolume;
            if (loadRenderVolume.RMSettings != null)
            {
                editorTendency = loadRenderVolume.RMSettings.Tendencies;
                if (loadRenderVolume.algorithm == ReactiveMesSettings.TendencyAlgorithm.Preset) // preset algo - index not ideal, name match how?
                {
                    tendencyIndex.intValue = EditorGUILayout.Popup("Preset Tendency:", tendencyIndex.intValue, editorTendency);   
                }
                presetTendency.stringValue = editorTendency[tendencyIndex.intValue];

                EditorGUILayout.PropertyField(requestType);

                editorLocale = loadRenderVolume.RMSettings.Locales;
                if (loadRenderVolume.requestType == ReactiveMesSettings.RequestType.Locale)
                {
                    localeIndex.intValue = EditorGUILayout.Popup("Locale to Request:", localeIndex.intValue, editorLocale);
                }
                localeRequest.stringValue = editorLocale[localeIndex.intValue];
            }
            if (loadRenderVolume.algorithm == ReactiveMesSettings.TendencyAlgorithm.Preset)
            {
                EditorGUILayout.PropertyField(presetVolumeProfile);
            }
            else
            {
                EditorGUILayout.PropertyField(volumeProfiles);
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
