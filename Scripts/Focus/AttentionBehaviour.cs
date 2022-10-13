using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public abstract class AttentionBehaviour : MonoBehaviour
    {
        public FocusNeue focusObj;
        public float[] thresholds;

        private float attentionRating;
        private float cumulativeAttentionRating;

        public virtual float AttentionRating { get => attentionRating; set => attentionRating = value; }
        public virtual float CumulativeAttentionRating { get => cumulativeAttentionRating; set => cumulativeAttentionRating = value; }

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

        public virtual bool CheckRatingAgainstThreshold (float rating, float threshold)
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

        public virtual Dictionary<float, bool> CheckAgainstMultipleThresholds (float rating, params float[] thresholds)
        {
            Dictionary<float, bool> thresholdChecks = new Dictionary<float, bool>();
            foreach (var threshold in thresholds)
            {
                if (rating > threshold)
                {
                    thresholdChecks.Add(threshold, true);
                }
                else
                {
                    thresholdChecks.Add(threshold, false);
                }
            }
            return thresholdChecks;
        }

        // Map a value from one range to another,
        // e.g. map a rating value to something useful for a property like material colour or video clip length...
        public virtual float MapValue(float inputValue, float fromMin, float fromMax, float toMin, float toMax)
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
