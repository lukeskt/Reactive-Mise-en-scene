using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    // Eventually roll these enums into the SO, and replace some with lists of strings? Would be more flexible?
    public enum Tendencies
    {
        Neutral,
        Spy,
        Terrorist,
        //Resistance,
        //Oneirica,
        Auteur
    }

    public enum Locales
    {
        WhitespaceLab,
        Apartment,
        Cafe,
        Gallery,
        Church,
        Underpass,
        Bunker,
        Datacentre,
        Denouement,
    }

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
        Proportional, // proportion of decision/placement points distroed amongst tendencies
        //ProportionalPastThePost, // any that pass a threshold get a proportional rep
        InverseProportion, // i.e. load inverse to what proportional results would be
        CompetitorDistribution, // distribute the tendency total amongst all others than the winning tendency
        Preset,
        Random
    }

    // This SO isn't working yet, but could provide inspector-accessible alt to hard-coded tendencies.
    //[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ReactiveMediaSettings", order = 1)]
    //public class ReactiveMediaSettings : ScriptableObject
    //{
    //    // Need to figure out how to replace enums above with lists, then dropdown those in custom inspector.
    //    //public List<string> Tendencies = new List<string>();
    //    //public List<string> Locales = new List<string>();

    //    // The three official SO built-in methods:
    //    private void Awake() { }
    //    private void OnEnable() { }
    //    private void OnValidate() { }

    //    // Define other methods below?
    //    public void TestFunc()
    //    {
    //        Debug.Log("testing reactivemediasettings SO!");
    //    }
    //}
}