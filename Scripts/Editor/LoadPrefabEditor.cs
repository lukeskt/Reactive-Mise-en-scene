#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(LoadPrefab))]
    [CanEditMultipleObjects]
    public class LoadPrefabEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty algorithm;
        SerializedProperty requestType;

        SerializedProperty localeRequest;
        string[] editorLocale;
        SerializedProperty localeIndex;

        SerializedProperty continuous;

        SerializedProperty tendencyObjects;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            algorithm = serializedObject.FindProperty("algorithm");
            requestType = serializedObject.FindProperty("requestType");
            var loadPrefabSingle = target as LoadPrefab;
            if (loadPrefabSingle.RMSettings != null)
            {
                editorLocale = loadPrefabSingle.RMSettings.Locales;
            }
            localeRequest = serializedObject.FindProperty("localeRequest");
            localeIndex = serializedObject.FindProperty("localeIndex");
            continuous = serializedObject.FindProperty("continuous");
            tendencyObjects = serializedObject.FindProperty("tendencyObjects");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            EditorGUILayout.PropertyField(algorithm);

            var loadPrefabSingle = target as LoadPrefab;

            if (loadPrefabSingle.RMSettings != null)
            {
                if (loadPrefabSingle.algorithm != ReactiveMesSettings.SingleResultTendencyAlgorithm.Random)
                {
                    EditorGUILayout.PropertyField(requestType);
                    editorLocale = loadPrefabSingle.RMSettings.Locales;
                    if (loadPrefabSingle.requestType == ReactiveMesSettings.RequestType.Locale)
                    {
                        localeIndex.intValue = EditorGUILayout.Popup("Locale to Request:", localeIndex.intValue, editorLocale);
                    }
                    localeRequest.stringValue = editorLocale[localeIndex.intValue];
                }
            }
            //EditorGUILayout.PropertyField(continuous); // uncomment to show continuous check.
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(tendencyObjects);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
