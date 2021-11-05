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
        
        //SerializedProperty presetTendency;
        //string[] editorTendency;
        //SerializedProperty tendencyIndex;

        SerializedProperty localeRequest;
        string[] editorLocale;
        SerializedProperty localeIndex;

        SerializedProperty continuous;

        SerializedProperty volumeProfiles;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            algorithm = serializedObject.FindProperty("algorithm");
            requestType = serializedObject.FindProperty("requestType");
            var loadRenderVolume = target as LoadRenderVolume;
            if (loadRenderVolume.RMSettings != null)
            {
                //editorTendency = loadRenderVolume.RMSettings.Tendencies;
                editorLocale = loadRenderVolume.RMSettings.Locales;
            }
            //presetTendency = serializedObject.FindProperty("presetTendency");
            //tendencyIndex = serializedObject.FindProperty("tendencyIndex");
            localeRequest = serializedObject.FindProperty("localeRequest");
            localeIndex = serializedObject.FindProperty("localeIndex");
            continuous = serializedObject.FindProperty("continuous");
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
                //editorTendency = loadRenderVolume.RMSettings.Tendencies;
                //presetTendency.stringValue = editorTendency[tendencyIndex.intValue];

                if (loadRenderVolume.algorithm != ReactiveMesSettings.SingleResultTendencyAlgorithm.Random)
                {
                    EditorGUILayout.PropertyField(requestType);
                    if (loadRenderVolume.requestType == ReactiveMesSettings.RequestType.Locale)
                    {
                        editorLocale = loadRenderVolume.RMSettings.Locales;
                        localeIndex.intValue = EditorGUILayout.Popup("Locale to Request:", localeIndex.intValue, editorLocale);
                        localeRequest.stringValue = editorLocale[localeIndex.intValue];
                    }
                }
            }
            //EditorGUILayout.PropertyField(continuous); // uncomment to show continuous check.
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(volumeProfiles);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif