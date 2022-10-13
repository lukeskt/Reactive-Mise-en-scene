using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public abstract class FocusBehaviour : MonoBehaviour
    {
        [SerializeField] private FocusNeue focusObj;
        [SerializeField] private float[] thresholds;

        private float attentionRating;
        private float cumulativeAttentionRating;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            // Every frame get a local copy of attention values.
            attentionRating = focusObj.getFocusValue;
            cumulativeAttentionRating = focusObj.getCumulativeFocusValue;
            // Then use in whatever methods.
        }

        private bool CheckRatingAgainstThreshold (float rating, float threshold)
        {
            if (rating > threshold)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private float MapValue(float inputValue, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = inputValue - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var mappedValue = toAbs + toMin;
            mappedValue = Mathf.Clamp(mappedValue, toMin, toMax);
            return mappedValue;
        }
    }
}
