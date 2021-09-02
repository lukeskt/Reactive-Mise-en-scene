using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(ObjectLocaleTendencyTags))]
    [CanEditMultipleObjects]
    public class ObjectLocaleTendencyTagsEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty locale;
        SerializedProperty localeIndex;
        public string[] editorLocale;
        SerializedProperty tendency;
        SerializedProperty tendencyIndex;
        public string[] editorTendency;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            locale = serializedObject.FindProperty("locale");
            localeIndex = serializedObject.FindProperty("localeIndex");
            tendency = serializedObject.FindProperty("tendency");
            tendencyIndex = serializedObject.FindProperty("tendencyIndex");
            var objectTags = target as ObjectLocaleTendencyTags;
            if (objectTags.RMSettings != null)
            {
                editorLocale = objectTags.RMSettings.Locales;
                editorTendency = objectTags.RMSettings.Tendencies;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            var objectTags = target as ObjectLocaleTendencyTags;
            if (objectTags.RMSettings != null)
            {
                editorLocale = objectTags.RMSettings.Locales;
                localeIndex.intValue = EditorGUILayout.Popup(label: "Locale:", localeIndex.intValue, editorLocale);
                locale.stringValue = editorLocale[localeIndex.intValue];

                editorTendency = objectTags.RMSettings.Tendencies;
                tendencyIndex.intValue = EditorGUILayout.Popup("Tendency:", tendencyIndex.intValue, editorTendency);
                tendency.stringValue = editorTendency[tendencyIndex.intValue];
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
