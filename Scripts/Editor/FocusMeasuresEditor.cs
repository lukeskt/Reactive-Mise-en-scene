using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(FocusMeasures))]
    [CanEditMultipleObjects]
    public class FocusMeasuresEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty locale;
        string[] editorLocale;
        int _localeIndex = 0;
        SerializedProperty tendency;
        string[] editorTendency;
        int _tendencyIndex = 0;
        SerializedProperty visibleMultiplier;
        SerializedProperty attendedMultiplier;
        SerializedProperty focusedMultiplier;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            locale = serializedObject.FindProperty("locale");
            tendency = serializedObject.FindProperty("tendency");
            var focusMeasures = target as FocusMeasures;
            if (focusMeasures.RMSettings != null)
            {
                editorLocale = focusMeasures.RMSettings.Locales;
                editorTendency = focusMeasures.RMSettings.Tendencies;
            }
            visibleMultiplier = serializedObject.FindProperty("visibleMultiplier");
            attendedMultiplier = serializedObject.FindProperty("attendedMultiplier");
            focusedMultiplier = serializedObject.FindProperty("focusedMultiplier");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            var focusMeasures = target as FocusMeasures;
            if (focusMeasures.RMSettings != null)
            {
                _localeIndex = EditorGUILayout.Popup("Locale to Request:", _localeIndex, editorLocale);
                locale.stringValue = editorLocale[_localeIndex];
                _tendencyIndex = EditorGUILayout.Popup("Preset Tendency:", _tendencyIndex, editorTendency);
                tendency.stringValue = editorTendency[_tendencyIndex];
            }
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(visibleMultiplier);
            EditorGUILayout.PropertyField(attendedMultiplier);
            EditorGUILayout.PropertyField(focusedMultiplier);
        }
    }
}
