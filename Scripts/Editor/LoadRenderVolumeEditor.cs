#if UNITY_EDITOR
using UnityEditor;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(LoadRenderVolume))]
    [CanEditMultipleObjects]
    public class LoadRenderVolumeEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty algorithm;
        SerializedProperty requestType;
        SerializedProperty presetTendency;
        string[] editorTendency;
        SerializedProperty tendencyIndex;
        string[] editorLocale;
        SerializedProperty localeRequest;
        SerializedProperty localeIndex;
        SerializedProperty volumeProfiles;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            algorithm = serializedObject.FindProperty("algorithm");
            var loadRenderVolume = target as LoadRenderVolume;
            if (loadRenderVolume.RMSettings != null)
            {
                editorTendency = loadRenderVolume.RMSettings.Tendencies;
                editorLocale = loadRenderVolume.RMSettings.Locales;
            }
            requestType = serializedObject.FindProperty("requestType");
            presetTendency = serializedObject.FindProperty("presetTendency");
            tendencyIndex = serializedObject.FindProperty("tendencyIndex");
            localeRequest = serializedObject.FindProperty("localeRequest");
            localeIndex = serializedObject.FindProperty("localeIndex");
            volumeProfiles = serializedObject.FindProperty("volumeProfiles");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            EditorGUILayout.PropertyField(algorithm);
            var loadRenderVolume = target as LoadRenderVolume;
            if (loadRenderVolume.RMSettings != null)
            {
                editorTendency = loadRenderVolume.RMSettings.Tendencies;
                presetTendency.stringValue = editorTendency[tendencyIndex.intValue];

                EditorGUILayout.PropertyField(requestType);

                editorLocale = loadRenderVolume.RMSettings.Locales;
                if (loadRenderVolume.requestType == ReactiveMesSettings.RequestType.Locale)
                {
                    localeIndex.intValue = EditorGUILayout.Popup("Locale to Request:", localeIndex.intValue, editorLocale);
                }
                localeRequest.stringValue = editorLocale[localeIndex.intValue];
            }
            EditorGUILayout.PropertyField(volumeProfiles);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif