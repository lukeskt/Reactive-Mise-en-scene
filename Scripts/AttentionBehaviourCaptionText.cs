using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Remes
{
    public class AttentionBehaviourCaptionText : MonoBehaviour
    {
        public AttentionTracker focusComponent;
        private float focusValue;

        public TMP_Text captionText;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            focusValue = focusComponent.getFocusValue;
            if (focusValue > 0.87)
            {
                captionText.text = "- I couldn't believe this was happening.";
            }
            else if (focusValue > 0.67)
            {
                captionText.text = "- Each breath I took became shorter.";
            }
            else if (focusValue > 0.37)
            {
                captionText.text = "- Something out of the corner of my eye...";
            }
            else if (focusValue < 0.01)
            {
                captionText.text = "";
            }
        }
    }
}
