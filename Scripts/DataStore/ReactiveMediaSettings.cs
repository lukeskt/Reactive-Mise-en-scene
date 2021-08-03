using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMedia
{
    // This SO isn't working yet, but could provide inspector-accessible alt to hard-coded tendencies.
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ReactiveMediaSettings", order = 1)]
    public class ReactiveMediaSettings : ScriptableObject
    {
        public List<string> Tendencies = new List<string>();
        public List<string> Locales = new List<string>();

        public enum RequestType
        {
            Global,
            Locale
        }

        public enum TendencyAlgorithm
        {
            MaxValue, // winning tendency by largest attention value
            MinValue,
            //FirstPastThePost, // winning tendency past a threshold
            Proportional, // proportion of decision/placement points given to winning tendency
            InverseProportion, // i.e. load an "opposite" tendency as specified as the larger
            CompetitorDistribution, // distribute the tendency total amongst all others than the winning tendency
            Preset,
            Random
        }

        // The three official SO built-in methods:
        private void Awake() { }
        private void OnEnable() { }
        private void OnValidate() { }

        // Define other methods below?
        public void TestFunc()
        {
            Debug.Log("testing reactivemediasettings SO!");
        }
    }
}