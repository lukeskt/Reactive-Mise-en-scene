using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class LoadSetDressing : MonoBehaviour
    {
        public ReactiveMediaSettings RMSettings;

        public ReactiveMediaSettings.RequestType requestType;
        public string locale;
        public string tendency;
        public ReactiveMediaSettings.TendencyAlgorithm algorithm;

        // Start is called before the first frame update
        void Start()
        {
            // get tendencies
            // load elements based on thresholds of each/compared tendencies?
            // Maybe build prefab chunks to do this?
        }
    }
}