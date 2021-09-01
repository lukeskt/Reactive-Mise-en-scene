#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class LoadTimelineEditor : Editor
    {
        SerializedProperty locale;
        SerializedProperty tendency;
        SerializedProperty timeline;

        private void OnEnable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
#endif