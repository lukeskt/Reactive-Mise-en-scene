#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(RemesPrefabLoader))]
    [CanEditMultipleObjects]
    public class RemesPrefabLoaderEditor : Editor
    {
        SerializedProperty RMSettings;
        SerializedProperty algorithm;
        SerializedProperty requestType;

        SerializedProperty localeRequest;
        string[] editorLocale;
        SerializedProperty localeIndex;

        SerializedProperty replaceObject;
        SerializedProperty loadOnStart;

        //SerializedProperty tendencyNames;
        string[] editorTendencyNames;
        SerializedProperty tendencyObjs;
        //GameObject[] editorTendencyObjs;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            algorithm = serializedObject.FindProperty("algorithm");
            requestType = serializedObject.FindProperty("requestType");
            var remesPrefabLoader = target as RemesPrefabLoader;
            if (remesPrefabLoader.RMSettings != null)
            {
                editorLocale = remesPrefabLoader.RMSettings.Locales;
                editorTendencyNames = remesPrefabLoader.tendencyNames;
                //editorTendencyObjs = loadPrefabSingle.tendencyObjs;
            }
            localeRequest = serializedObject.FindProperty("localeRequest");
            localeIndex = serializedObject.FindProperty("localeIndex");
            replaceObject = serializedObject.FindProperty("replaceObject");
            loadOnStart = serializedObject.FindProperty("loadOnStart");

            //tendencyNames = serializedObject.FindProperty("tendencyNames");
            tendencyObjs = serializedObject.FindProperty("tendencyObjs");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(RMSettings);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            EditorGUILayout.PropertyField(algorithm);

            var remesPrefabLoader = target as RemesPrefabLoader;

            if (remesPrefabLoader.RMSettings != null)
            {
                if (remesPrefabLoader.algorithm != RemesSettings.SingleResultTendencyAlgorithm.Random)
                {
                    EditorGUILayout.PropertyField(requestType);
                    editorLocale = remesPrefabLoader.RMSettings.Locales;
                    if (remesPrefabLoader.requestType == RemesSettings.RequestType.Locale)
                    {
                        localeIndex.intValue = EditorGUILayout.Popup("Locale to Request:", localeIndex.intValue, editorLocale);
                    }
                    localeRequest.stringValue = editorLocale[localeIndex.intValue];
                }
            }
            EditorGUILayout.PropertyField(replaceObject);
            EditorGUILayout.PropertyField(loadOnStart);
            EditorGUILayout.Space();

            for (int i = 0; i < editorTendencyNames.Length; i++)
            {
                EditorGUILayout.PropertyField(tendencyObjs.GetArrayElementAtIndex(i), label: new GUIContent(editorTendencyNames[i]));
            }

            //foreach (var name in editorTendencyNames)
            //{
            //    var pos = Array.IndexOf(editorTendencyNames, name);
            //    GUIContent lab = new GUIContent(editorTendencyNames[pos]);
            //    EditorGUILayout.PropertyField(tendencyObjs.GetArrayElementAtIndex(pos), label: lab);
            //}

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
