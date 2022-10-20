using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Remes
{
    public class AttentionBehaviourCaptionText : MonoBehaviour
    {
        public AttentionTracker attentionTracker;
        private float attentionRating;

        public TMP_Text captionText;

        // Start is called before the first frame update
        void Start()
        {
            captionText.text = "";
        }

        // Update is called once per frame
        void Update()
        {
            attentionRating = attentionTracker.getFocusValue;
            if (attentionRating > 0.87)
            {
                captionText.text = "- I couldn't believe this was happening.";
            }
            else if (attentionRating > 0.67)
            {
                captionText.text = "- Each breath I took became shorter.";
            }
            else if (attentionRating > 0.37)
            {
                captionText.text = "- Something out of the corner of my eye...";
            }
            else if (attentionRating < 0.01)
            {
                captionText.text = "";
            }
        }
    }
}
