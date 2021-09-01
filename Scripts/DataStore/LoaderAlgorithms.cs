using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class LoaderAlgorithms : ScriptableObject
    {

        public class SingleResultAlgorithms
        {
            public ReactiveMesSettings RMSettings;

            public enum Algorithms
            {
                MaxValue,
                MinValue,
                // FirstPastThreshold, // winning past threshold,
                Random
            }

            public string MaxValue ()
            {
                string maxTendency = "help";
                return maxTendency;
            }

            public string MinValue()
            {
                string minTendency = "help";
                return minTendency;
            }

            public string RandomSelection()
            {
                string randomTendency = RMSettings.Tendencies[Random.Range(0, RMSettings.Tendencies.Length)];
                return randomTendency;
            }
        }

        SingleResultAlgorithms singleResultAlgorithms = new SingleResultAlgorithms();

        public class MultiResultAlgorithms
        {
            public enum Algorithms
            {
                MaxValue,
                MinValue,
                // FirstPastThreshold, // winning past threshold
                Proportional,
                // ProportionalPastThreshold, // Any past a threshold get a proportional representation
                InverseProportional,
                // Competitor Distribution, // distribute the tendency total amongst all others than the winning tendency
                Preset,
                Random
            }

            public List<string> RequestResults (Algorithms algorithm, int listLength)
            {
                List<string> results;
                switch (algorithm)
                {
                    case Algorithms.MaxValue:
                        results = MaxValue(listLength);
                        break;
                    case Algorithms.MinValue:
                        results = MinValue(listLength);
                        break;
                    case Algorithms.Proportional:
                        results = Proportional(listLength);
                        break;
                    case Algorithms.InverseProportional:
                        results = InverseProportional(listLength);
                        break;
                    case Algorithms.Preset:
                        results = Preset(listLength);
                        break;
                    case Algorithms.Random:
                        results = Random(listLength);
                        break;
                    default:
                        goto case Algorithms.Preset;
                }
                return results;
            }

            public List<string> MaxValue (int listLength)
            {
                List<string> results = new List<string>();

                return results;
            }

            public List<string> MinValue (int listLength)
            {
                List<string> results = new List<string>();

                return results;
            }

            public List<string> Proportional (int listLength)
            {
                List<string> results = new List<string>();

                return results;
            }

            public List<string> InverseProportional (int listLength)
            {
                List<string> results = new List<string>();

                return results;
            }

            public List<string> Preset (int listLength)
            {
                List<string> results = new List<string>();

                return results;
            }

            public List<string> Random (int listLength)
            {
                List<string> results = new List<string>();

                return results;
            }
        }

        MultiResultAlgorithms multiResultAlgorithms = new MultiResultAlgorithms();
    }
}
