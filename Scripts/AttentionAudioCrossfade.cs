using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class AttentionAudioCrossfade : MonoBehaviour
    {
        public AttentionTracker focusComponent;
        private float focusValue;

        public AudioSource audioSrc1;
        public AudioSource audioSrc2;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            focusValue = focusComponent.getFocusValue;
            CrossfadeAudio();
        }

        private void CrossfadeAudio()
        {
            audioSrc1.volume = Mathf.InverseLerp(1, 0, focusValue);
            audioSrc2.volume = Mathf.InverseLerp(0, 1, focusValue);
        }
    }
}
