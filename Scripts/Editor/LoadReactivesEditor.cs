#if UNITY_EDITOR
using UnityEditor;

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
        SerializedProperty tendencyIndex;
        SerializedProperty requestType;
        SerializedProperty localeRequest;
        string[] editorLocale;
        SerializedProperty localeIndex;
        SerializedProperty listOfTendencyPlacements;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            tendencyAlgorithm = serializedObject.FindProperty("tendencyAlgorithm");
            presetTendency = serializedObject.FindProperty("presetTendency");
            tendencyIndex = serializedObject.FindProperty("tendencyIndex");
            localeRequest = serializedObject.FindProperty("localeRequest");
            localeIndex = serializedObject.FindProperty("localeIndex");
            var loadReactives = target as LoadReactives;
            if (loadReactives.RMSettings != null)
            {
                editorTendency = loadReactives.RMSettings.Tendencies;
                editorLocale = loadReactives.RMSettings.Locales;
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
            EditorGUILayout.PropertyField(tendencyAlgorithm);
            var loadReactives = target as LoadReactives;
            if (loadReactives.RMSettings != null)
            {
                editorTendency = loadReactives.RMSettings.Tendencies;
                if (loadReactives.tendencyAlgorithm == ReactiveMesSettings.TendencyAlgorithm.Preset) // preset algo - index not ideal, name match how?
                {
                    tendencyIndex.intValue = EditorGUILayout.Popup(label:"Preset Tendency:", tendencyIndex.intValue, editorTendency);
                }
                presetTendency.stringValue = editorTendency[tendencyIndex.intValue];

                EditorGUILayout.PropertyField(requestType);
                
                editorLocale = loadReactives.RMSettings.Locales;
                if (loadReactives.requestType == ReactiveMesSettings.RequestType.Locale)
                {
                    localeIndex.intValue = EditorGUILayout.Popup(label:"Locale to Request:", localeIndex.intValue, editorLocale);
                }
                localeRequest.stringValue = editorLocale[localeIndex.intValue];
            }
            EditorGUILayout.PropertyField(listOfTendencyPlacements);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif