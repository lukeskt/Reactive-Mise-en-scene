
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CustomEditor(typeof(LoadSetDressing))]
    [CanEditMultipleObjects]
    public class LoadSetDressingEditor : Editor
    {
        SerializedProperty RMSettings;
        ReactiveMesSettings.RequestType editorRequestType;
        SerializedProperty locale;
        string[] editorLocale;
        int _localeIndex = 0;
        SerializedProperty tendency;
        string[] editorTendency;
        int _tendencyIndex = 0;
        ReactiveMesSettings.TendencyAlgorithm algorithm;

        private void OnEnable()
        {
            RMSettings = serializedObject.FindProperty("RMSettings");
            if (RMSettings != null)
            {
                var setDressing = target as LoadSetDressing;
                editorRequestType = setDressing.requestType;
                editorLocale = setDressing.RMSettings.Locales;
                editorTendency = setDressing.RMSettings.Tendencies;
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(RMSettings);
            editorRequestType = (ReactiveMesSettings.RequestType)EditorGUILayout.EnumPopup(label:"Request Type:", editorRequestType);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            var setDressing = target as LoadSetDressing;
            if (setDressing.RMSettings != null)
            {
                editorLocale = setDressing.RMSettings.Locales;
                editorTendency = setDressing.RMSettings.Tendencies;
                if (editorRequestType == ReactiveMesSettings.RequestType.Locale)
                {
                    _localeIndex = EditorGUILayout.Popup("Locale:", _localeIndex, editorLocale);
                }
                _tendencyIndex = EditorGUILayout.Popup("Tendency:", _tendencyIndex, editorTendency);
                locale.stringValue = editorLocale[_localeIndex];
                tendency.stringValue = editorTendency[_tendencyIndex];
            }
            algorithm = (ReactiveMesSettings.TendencyAlgorithm)EditorGUILayout.EnumPopup("Algorithm:", algorithm);
            EditorUtility.SetDirty(target);
            //EditorGUILayout.PropertyField(tendency);
            serializedObject.ApplyModifiedProperties();
            //EditorGUILayout.Space();
            //DrawDefaultInspector();
        }
    }
}
