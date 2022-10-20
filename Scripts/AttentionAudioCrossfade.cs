using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remes
{
    public class AttentionAudioCrossfade : MonoBehaviour
    {
        public AttentionTracker attentionTracker;
        private float attentionRating;

        public AudioSource audioSrc1;
        public AudioSource audioSrc2;
        public AudioSource audioSrc3;

        private bool clickOffPlayed;
        private bool clickOnPlayed;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            attentionRating = attentionTracker.getFocusValue;
            CrossfadeAudio();
        }

        private void CrossfadeAudio()
        {
            if (attentionRating <= 0)
            {
                audioSrc1.volume = 0;
                audioSrc2.volume = 0;
                clickOnPlayed = false;
                if (!clickOffPlayed)
                {
                    audioSrc3.Play();
                    clickOffPlayed = true;
                }
            }
            else
            {
                clickOffPlayed = false;
                if (!clickOnPlayed)
                {
                    audioSrc3.Play();
                    clickOnPlayed = true;
                }
                audioSrc1.volume = Mathf.InverseLerp(1, 0, attentionRating);
                audioSrc2.volume = Mathf.InverseLerp(0, 1, attentionRating);
            }
        }
    }
}
