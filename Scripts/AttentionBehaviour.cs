using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remes
{
    public abstract class AttentionBehaviour : MonoBehaviour
    {
        public AttentionTracker attentionTracker;
        //public float[] thresholds;

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
            attentionRating = attentionTracker.getFocusValue;
            cumulativeAttentionRating = attentionTracker.getCumulativeFocusValue;
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
        public virtual float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
