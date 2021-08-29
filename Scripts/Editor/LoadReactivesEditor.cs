using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(LoadReactives))]
    [CanEditMultipleObjects]
    public class LoadReactivesEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty tendencyAlgorithm;
        SerializedProperty presetTendency;
        string[] editorTendency;
        int _tendencyIndex = 0;
        SerializedProperty requestType;
        SerializedProperty localeRequest;
        string[] editorLocale;
        int _localeIndex = 0;
        SerializedProperty listOfTendencyPlacements;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            tendencyAlgorithm = serializedObject.FindProperty("tendencyAlgorithm");
            presetTendency = serializedObject.FindProperty("presetTendency");
            var loadReactives = target as LoadReactives;
            if (loadReactives.RMSettings != null)
            {
                editorTendency = loadReactives.RMSettings.Tendencies;
                editorLocale = loadReactives.RMSettings.Locales;
            }
            requestType = serializedObject.FindProperty("requestType");
            localeRequest = serializedObject.FindProperty("localeRequest");
            listOfTendencyPlacements = serializedObject.FindProperty("listOfTendencyPlacements");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //DrawDefaultInspector();
            //EditorGUILayout.Space();
            //EditorGUILayout.Space();
            //EditorGUILayout.LabelField("Custom Inspector Below");
            
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            EditorGUILayout.PropertyField(tendencyAlgorithm);
            var loadReactives = target as LoadReactives;
            if (loadReactives.RMSettings != null)
            {
                if (loadReactives.tendencyAlgorithm == ReactiveMesSettings.TendencyAlgorithm.Preset) // preset algo - index not ideal, name match how?
                {
                    editorTendency = loadReactives.RMSettings.Tendencies;
                    _tendencyIndex = EditorGUILayout.Popup("Preset Tendency:", _tendencyIndex, editorTendency);
                    presetTendency.stringValue = editorTendency[_tendencyIndex];
                }
                EditorGUILayout.PropertyField(requestType);
                if (loadReactives.requestType == ReactiveMesSettings.RequestType.Locale)
                {
                    editorLocale = loadReactives.RMSettings.Locales;
                    _localeIndex = EditorGUILayout.Popup("Locale to Request:", _localeIndex, editorLocale);
                    localeRequest.stringValue = editorLocale[_localeIndex];
                }
            }
            EditorGUILayout.PropertyField(listOfTendencyPlacements);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
