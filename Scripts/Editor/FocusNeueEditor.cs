#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(FocusNeue))]
    [CanEditMultipleObjects]
    public class FocusNeueEditor : Editor
    {
        SerializedProperty RMSettings;
        public string[] editorLocale;
        SerializedProperty locale;
        SerializedProperty localeIndex;
        public string[] editorTendency;
        SerializedProperty tendency;
        SerializedProperty tendencyIndex;
        SerializedProperty distanceThreshold;
        SerializedProperty cam;

        // Start is called before the first frame update
        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            locale = serializedObject.FindProperty("locale");
            localeIndex = serializedObject.FindProperty("localeIndex");
            tendency = serializedObject.FindProperty("tendency");
            tendencyIndex = serializedObject.FindProperty("tendencyIndex");
            var focusNeue = target as FocusNeue;
            if (focusNeue.RMSettings != null)
            {
                editorLocale = focusNeue.RMSettings.Locales;
                editorTendency = focusNeue.RMSettings.Tendencies;
            }
            distanceThreshold = serializedObject.FindProperty("distanceThreshold");
            cam = serializedObject.FindProperty("cam");
        }

        // Update is called once per frame
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            var focusNeue = target as FocusNeue;
            if (focusNeue.RMSettings != null)
            {
                editorLocale = focusNeue.RMSettings.Locales;
                localeIndex.intValue = EditorGUILayout.Popup(label: "Locale", localeIndex.intValue, editorLocale);
                locale.stringValue = editorLocale[localeIndex.intValue];

                editorTendency = focusNeue.RMSettings.Tendencies;
                tendencyIndex.intValue = EditorGUILayout.Popup("Tendency", tendencyIndex.intValue, editorTendency);
                tendency.stringValue = editorTendency[tendencyIndex.intValue];
            }
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(distanceThreshold);
            EditorGUILayout.PropertyField(cam);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
