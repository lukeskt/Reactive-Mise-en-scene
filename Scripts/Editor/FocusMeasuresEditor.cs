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
        public string[] editorLocale;
        SerializedProperty locale;
        SerializedProperty localeIndex;
        public string[] editorTendency;
        SerializedProperty tendency;
        SerializedProperty tendencyIndex;
        SerializedProperty visibleMultiplier;
        SerializedProperty attendedMultiplier;
        SerializedProperty focusedMultiplier;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            locale = serializedObject.FindProperty("locale");
            localeIndex = serializedObject.FindProperty("localeIndex");
            tendency = serializedObject.FindProperty("tendency");
            tendencyIndex = serializedObject.FindProperty("tendencyIndex");
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
            serializedObject.Update();
            //DrawDefaultInspector();
            //EditorGUILayout.Space();
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            var focusMeasures = target as FocusMeasures;
            if (focusMeasures.RMSettings != null)
            {
                editorLocale = focusMeasures.RMSettings.Locales;
                editorTendency = focusMeasures.RMSettings.Tendencies;
                localeIndex.intValue = EditorGUILayout.Popup(label:"Locale:", localeIndex.intValue, editorLocale);
                locale.stringValue = editorLocale[localeIndex.intValue];
                tendencyIndex.intValue = EditorGUILayout.Popup("Tendency:", tendencyIndex.intValue, editorTendency);
                tendency.stringValue = editorTendency[tendencyIndex.intValue];
            }
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(visibleMultiplier);
            EditorGUILayout.PropertyField(attendedMultiplier);
            EditorGUILayout.PropertyField(focusedMultiplier);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
