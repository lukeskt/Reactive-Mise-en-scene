#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(DestroyObjects))]
    [CanEditMultipleObjects]
    public class DestroyObjectsEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty tendencyAlgorithm;
        SerializedProperty requestType;
        SerializedProperty tendencyListToDestroy;
        public string[] editorTendency;
        SerializedProperty tendencyIndex;
        SerializedProperty localeRequest;
        SerializedProperty localeIndex;
        public string[] editorLocale;
        SerializedProperty TendencyObjects;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            tendencyAlgorithm = serializedObject.FindProperty("tendencyAlgorithm");
            requestType = serializedObject.FindProperty("requestType");
            tendencyListToDestroy = serializedObject.FindProperty("tendencyListToDestroy");
            tendencyIndex = serializedObject.FindProperty("tendencyIndex");
            var destroyObjects = target as DestroyObjects;
            if (destroyObjects.RMSettings != null)
            {
                editorLocale = destroyObjects.RMSettings.Locales;
                editorTendency = destroyObjects.RMSettings.Tendencies;
            }
            localeRequest = serializedObject.FindProperty("localeRequest");
            localeIndex = serializedObject.FindProperty("localeIndex");
            TendencyObjects = serializedObject.FindProperty("TendencyObjects");
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
            EditorGUILayout.PropertyField(requestType);
            var destroyObjects = target as DestroyObjects;
            if (destroyObjects.RMSettings != null)
            {
                editorLocale = destroyObjects.RMSettings.Locales;
                localeIndex.intValue = EditorGUILayout.Popup(label: "Locale:", localeIndex.intValue, editorLocale);
                localeRequest.stringValue = editorLocale[localeIndex.intValue];

                editorTendency = destroyObjects.RMSettings.Tendencies;
                tendencyIndex.intValue = EditorGUILayout.Popup("Tendency:", tendencyIndex.intValue, editorTendency);
                tendencyListToDestroy.stringValue = editorTendency[tendencyIndex.intValue];
            }
            EditorGUILayout.PropertyField(TendencyObjects);
            // Somehow define the tendencies in the tendencyobjects nested list?
            serializedObject.ApplyModifiedProperties();
        }
    }

    //[CustomPropertyDrawer(typeof(DestroyObjects.tendencyPrefabs))]
    //public class tendencyPrefabsDrawer : PropertyDrawer
    //{
        
    //}
}
#endif