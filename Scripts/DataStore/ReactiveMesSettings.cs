using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CreateAssetMenu(fileName = "Data", menuName = "ReactiveMesSettings", order = 1)]
    public class ReactiveMesSettings : ScriptableObject
    {
        public string[] Locales = new string[4];
        public string[] Tendencies = new string[4];

        public enum RequestType
        {
            Global,
            Locale
        }
        public enum SingleResultTendencyAlgorithm
        {
            StrongestTendency,
            SecondStrongest,
            SecondWeakest,
            WeakestTendency,
            Random
        }

        // The three official SO built-in methods:
        private void Awake() { }
        private void OnEnable() { }
        private void OnValidate() { }

        public void TestFunc()
        {
            Debug.Log("Reactive Mise-en-scene Settings SO Test");
        }
    }
}