using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    [CreateAssetMenu(fileName = "Data", menuName = "ReactiveMesSettings", order = 1)]
    public class ReactiveMesSettings : ScriptableObject
    {
        // Need to figure out how to replace enums above with lists, then dropdown those in custom inspector.
        public string[] Locales = new string[4];
        public string[] Tendencies = new string[4];

        public enum RequestType
        {
            Global,
            Locale
        }

        //public enum MultiResultTendencyAlgorithm
        //{
        //    MaxValue, // winning tendency by largest attention value
        //    RunnerUp, // second place tendency
        //    MinValue,
        //    // FirstPastThreshold, // winning tendency past a threshold
        //    Proportional, // proportion of decision/placement points distroed amongst tendencies
        //    // ProportionalPastThreshold, // any that pass a threshold get a proportional rep
        //    InverseProportion, // i.e. load inverse to what proportional results would be
        //    // CompetitorDistribution, // distribute the tendency total amongst all others than the winning tendency
        //    Preset,
        //    Random
        //}

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

    //    // Define other methods below?
        public void TestFunc()
        {
            Debug.Log("testing reactivemediasettings SO!");
        }
    }
}