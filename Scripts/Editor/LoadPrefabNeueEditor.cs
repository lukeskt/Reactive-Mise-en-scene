#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(LoadPrefabNeue))]
    [CanEditMultipleObjects]
    public class LoadPrefabNeueEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty algorithm;
        SerializedProperty requestType;

        SerializedProperty localeRequest;
        string[] editorLocale;
        SerializedProperty localeIndex;

    //    public string[] editorTendency;
    //    SerializedProperty tendency;
    //    SerializedProperty tendencyIndex;

        SerializedProperty replaceObject;
        SerializedProperty loadOnStart;

        SerializedProperty tendencyNames;
        string[] editorTendencyNames;
        SerializedProperty tendencyObjs;
        GameObject[] editorTendencyObjs;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            algorithm = serializedObject.FindProperty("algorithm");
            requestType = serializedObject.FindProperty("requestType");
            var loadPrefabSingle = target as LoadPrefabNeue;
            if (loadPrefabSingle.RMSettings != null)
            {
                editorLocale = loadPrefabSingle.RMSettings.Locales;
                editorTendencyNames = loadPrefabSingle.tendencyNames;
                editorTendencyObjs = loadPrefabSingle.tendencyObjs;
            }
            localeRequest = serializedObject.FindProperty("localeRequest");
            localeIndex = serializedObject.FindProperty("localeIndex");
            replaceObject = serializedObject.FindProperty("replaceObject");
            loadOnStart = serializedObject.FindProperty("loadOnStart");

            tendencyNames = serializedObject.FindProperty("tendencyNames");
            tendencyObjs = serializedObject.FindProperty("tendencyObjs");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            EditorGUILayout.PropertyField(algorithm);

            var loadPrefabSingle = target as LoadPrefabNeue;

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
            EditorGUILayout.PropertyField(replaceObject);
            EditorGUILayout.PropertyField(loadOnStart);
            EditorGUILayout.Space();

            //EditorGUILayout.PropertyField(tendencyNames);
            EditorGUI.BeginChangeCheck();
            foreach (var obj in editorTendencyObjs)
            {
                var pos = Array.IndexOf(editorTendencyObjs, obj);
                EditorGUILayout.PropertyField(tendencyObjs.GetArrayElementAtIndex(pos), label: new GUIContent(editorTendencyNames[pos]));
            }
            EditorGUI.EndChangeCheck();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
