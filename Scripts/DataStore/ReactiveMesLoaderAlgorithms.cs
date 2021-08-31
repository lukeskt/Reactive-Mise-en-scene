using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class ReactiveMesLoaderAlgorithms : ScriptableObject
    {
        public class SingleResultAlgorithms
        {
            public enum Algorithms
            {
                MaxValue,
                MinValue,
                Preset,
                Random
            }

            public string MaxValue ()
            {
                return "help";
            }

            public string MinValue()
            {
                return "help";
            }

            public string Preset()
            {
                return "help";
            }

            public string Random()
            {
                return "help";
            }
        }

        public class MultiResultAlgorithms
        {
            public enum Algorithms
            {
                MaxValue,
                MinValue,
                Proportional,
                InverseProportional,
                Preset,
                Random
            }

            public List<string> MaxValue ()
            {
                List<string> results = new List<string>();

                return results;
            }

            public List<string> MinValue()
            {
                List<string> results = new List<string>();

                return results;
            }

            public List<string> Proportional()
            {
                List<string> results = new List<string>();

                return results;
            }

            public List<string> InverseProportional()
            {
                List<string> results = new List<string>();

                return results;
            }

            public List<string> Preset()
            {
                List<string> results = new List<string>();

                return results;
            }

            public List<string> Random()
            {
                List<string> results = new List<string>();

                return results;
            }
        }
    }
}
