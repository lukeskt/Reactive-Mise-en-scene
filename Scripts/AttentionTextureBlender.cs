using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class AttentionTextureBlender : MonoBehaviour
    {
        public AttentionTracker focusComponent;
        private float focusValue;
        private float cumulativeFocus;

        public Material mat;

        // Update is called once per frame
        void Update()
        {
            focusValue = focusComponent.getFocusValue;
            cumulativeFocus = focusComponent.getCumulativeFocusValue;
            //var blendValue = Mathf.InverseLerp(1, 10, focusValue);
            //var blendValue = focusValue * 2;
            var blendValue = MapValue(focusValue, 0, 1, 1, 10);
            var clampedCumulative = Mathf.Clamp(cumulativeFocus, 1, 20);
            //print(blendValue);
            mat.SetFloat("_BlendOpacity", clampedCumulative);
        }

        private float MapValue (float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
