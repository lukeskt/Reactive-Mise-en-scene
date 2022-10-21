using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remes
{
    public class AttentionColourChanger : AttentionBehaviour
    {
        public Renderer rend;

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            ChangeColour();
        }

        private void ChangeColour ()
        {
            var mixer = Mathf.InverseLerp(1, 0, (float)AttentionRating);
            Color customColour = new Color((float)(AttentionRating * 0.5f), mixer, 0);
            rend.material.SetColor("_BaseColor", customColour);
        }

        private void ColourSteps()
        {
            if (AttentionRating == 0)
            {
                rend.material.SetColor("_BaseColor", Color.black);
            }
            else if (AttentionRating < 0.7f)
            {
                rend.material.SetColor("_BaseColor", Color.green);
            }
            else if (AttentionRating < 0.9f)
            {
                rend.material.SetColor("_BaseColor", Color.yellow);
            }
            else if (AttentionRating > 0.9f)
            {
                rend.material.SetColor("_BaseColor", Color.red);
            }
        }
    }
}
