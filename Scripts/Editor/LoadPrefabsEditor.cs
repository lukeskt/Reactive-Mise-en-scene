using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(LoadPrefabs))]
    [CanEditMultipleObjects]
    public class LoadPrefabsEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty algorithm;
        SerializedProperty presetTendency;
        string[] editorTendency;
        SerializedProperty tendencyIndex;
        SerializedProperty requestType;
        SerializedProperty localeRequest;
        string[] editorLocale;
        SerializedProperty localeIndex;
        SerializedProperty listOfTendencyPlacements;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            algorithm = serializedObject.FindProperty("tendencyAlgorithm");
            presetTendency = serializedObject.FindProperty("presetTendency");
            tendencyIndex = serializedObject.FindProperty("tendencyIndex");
            localeRequest = serializedObject.FindProperty("localeRequest");
            localeIndex = serializedObject.FindProperty("localeIndex");
            var loadPrefabs = target as LoadPrefabs;
            if (loadPrefabs.RMSettings != null)
            {
                editorTendency = loadPrefabs.RMSettings.Tendencies;
                editorLocale = loadPrefabs.RMSettings.Locales;
            }
            requestType = serializedObject.FindProperty("requestType");
            listOfTendencyPlacements = serializedObject.FindProperty("listOfTendencyPlacements");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            //DrawDefaultInspector();
            //EditorGUILayout.Space();
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            EditorGUILayout.PropertyField(algorithm);
            var loadPrefabs = target as LoadPrefabs;
            if (loadPrefabs.RMSettings != null)
            {
                if (loadPrefabs.tendencyAlgorithm == ReactiveMesSettings.TendencyAlgorithm.Preset) // preset algo - index not ideal, name match how?
                {
                    editorTendency = loadPrefabs.RMSettings.Tendencies;
                    tendencyIndex.intValue = EditorGUILayout.Popup("Preset Tendency:", tendencyIndex.intValue, editorTendency);
                    presetTendency.stringValue = editorTendency[tendencyIndex.intValue];
                }
                EditorGUILayout.PropertyField(requestType);
                if (loadPrefabs.requestType == ReactiveMesSettings.RequestType.Locale)
                {
                    editorLocale = loadPrefabs.RMSettings.Locales;
                    localeIndex.intValue = EditorGUILayout.Popup("Locale to Request:", localeIndex.intValue, editorLocale);
                    localeRequest.stringValue = editorLocale[localeIndex.intValue];
                }
            }
            EditorGUILayout.PropertyField(listOfTendencyPlacements);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
