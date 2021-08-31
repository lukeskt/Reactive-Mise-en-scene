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
        string[] editorTendency;
        int _tendencyIndex = 0;
        SerializedProperty requestType;
        string[] editorLocale;
        int _localeIndex = 0;
        SerializedProperty listOfTendencyPlacements;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            algorithm = serializedObject.FindProperty("tendencyAlgorithm");
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
            //EditorGUILayout.Space();
            //EditorGUILayout.LabelField("Custom Inspector Below");

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
                    _tendencyIndex = EditorGUILayout.Popup("Preset Tendency:", _tendencyIndex, editorTendency);
                    loadPrefabs.presetTendency = editorTendency[_tendencyIndex];
                }
                EditorGUILayout.PropertyField(requestType);
                if (loadPrefabs.requestType == ReactiveMesSettings.RequestType.Locale)
                {
                    editorLocale = loadPrefabs.RMSettings.Locales;
                    _localeIndex = EditorGUILayout.Popup("Locale to Request:", _localeIndex, editorLocale);
                    loadPrefabs.localeRequest = editorLocale[_localeIndex];
                }
            }
            EditorGUILayout.PropertyField(listOfTendencyPlacements);
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
