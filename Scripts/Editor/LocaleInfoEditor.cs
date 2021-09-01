#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(LocaleInfo))]
    [CanEditMultipleObjects]
    public class LocaleInfoEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty locale;
        SerializedProperty localeIndex;
        string[] editorLocale;
        SerializedProperty tendency;
        SerializedProperty tendencyRating;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            locale = serializedObject.FindProperty("locale");
            localeIndex = serializedObject.FindProperty("localeIndex");
            var localeInfo = target as LocaleInfo;
            if (localeInfo.RMSettings != null)
            {
                editorLocale = localeInfo.RMSettings.Locales;
            }
            tendency = serializedObject.FindProperty("tendency");
            tendencyRating = serializedObject.FindProperty("tendencyRating");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            var localeInfo = target as LocaleInfo;
            if (localeInfo.RMSettings != null)
            {
                editorLocale = localeInfo.RMSettings.Locales;
                localeIndex.intValue = EditorGUILayout.Popup(label: "Locale:", localeIndex.intValue, editorLocale);
                locale.stringValue = editorLocale[localeIndex.intValue];
            }
            EditorGUILayout.SelectableLabel($"Tendency: {tendency.stringValue}\nTendency Rating: {tendencyRating.doubleValue}");
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif