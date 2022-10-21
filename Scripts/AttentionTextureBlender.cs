using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remes
{
    public class AttentionTextureBlender : AttentionBehaviour
    {
        public Material mat;

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            var blendValue = MapValue((float)AttentionRating, 0, 1, 1, 10);
            var clampedCumulative = Mathf.Clamp((float)CumulativeAttentionRating, 1, 20);
            mat.SetFloat("_BlendOpacity", clampedCumulative);
        }
    }
}
