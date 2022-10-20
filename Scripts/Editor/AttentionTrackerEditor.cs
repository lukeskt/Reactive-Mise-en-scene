#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Remes
{
    [CustomEditor(typeof(AttentionTracker))]
    [CanEditMultipleObjects]
    public class AttentionTrackerEditor : Editor
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
            var attentionTracker = target as AttentionTracker;
            if (attentionTracker.RMSettings != null)
            {
                editorLocale = attentionTracker.RMSettings.Locales;
                editorTendency = attentionTracker.RMSettings.Tendencies;
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
            var attentionTracker = target as AttentionTracker;
            if (attentionTracker.RMSettings != null)
            {
                editorLocale = attentionTracker.RMSettings.Locales;
                localeIndex.intValue = EditorGUILayout.Popup(label: "Locale", localeIndex.intValue, editorLocale);
                locale.stringValue = editorLocale[localeIndex.intValue];

                editorTendency = attentionTracker.RMSettings.Tendencies;
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
