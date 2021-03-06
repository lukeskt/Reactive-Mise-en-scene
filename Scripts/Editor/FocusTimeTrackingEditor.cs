#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(FocusTimeTracking))]
    [CanEditMultipleObjects]
    public class FocusTimeTrackingEditor : Editor
    {
        SerializedProperty RMSettings;
        public string[] editorLocale;
        SerializedProperty locale;
        SerializedProperty localeIndex;
        public string[] editorTendency;
        SerializedProperty tendency;
        SerializedProperty tendencyIndex;
        SerializedProperty onscreenMultiplier;
        SerializedProperty attendedMultiplier;
        SerializedProperty focusedMultiplier;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            locale = serializedObject.FindProperty("locale");
            localeIndex = serializedObject.FindProperty("localeIndex");
            tendency = serializedObject.FindProperty("tendency");
            tendencyIndex = serializedObject.FindProperty("tendencyIndex");
            var focusMeasures = target as FocusTimeTracking;
            if (focusMeasures.RMSettings != null)
            {
                editorLocale = focusMeasures.RMSettings.Locales;
                editorTendency = focusMeasures.RMSettings.Tendencies;
            }
            onscreenMultiplier = serializedObject.FindProperty("onscreenMultiplier");
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
            var focusMeasures = target as FocusTimeTracking;
            if (focusMeasures.RMSettings != null)
            {
                editorLocale = focusMeasures.RMSettings.Locales;
                localeIndex.intValue = EditorGUILayout.Popup(label:"Locale:", localeIndex.intValue, editorLocale);
                locale.stringValue = editorLocale[localeIndex.intValue];

                editorTendency = focusMeasures.RMSettings.Tendencies;
                tendencyIndex.intValue = EditorGUILayout.Popup("Tendency:", tendencyIndex.intValue, editorTendency);
                tendency.stringValue = editorTendency[tendencyIndex.intValue];
            }
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(onscreenMultiplier);
            EditorGUILayout.PropertyField(attendedMultiplier);
            EditorGUILayout.PropertyField(focusedMultiplier);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif