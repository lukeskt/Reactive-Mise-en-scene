using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remes
{
    public abstract class AttentionBehaviour : MonoBehaviour
    {
        [field: SerializeField] public AttentionTracker AttentionTracker { get; set; }
        //public float[] thresholds;

        private float? attentionRating = null;
        private float? cumulativeAttentionRating = null;

        public virtual float? AttentionRating { get => attentionRating; set => attentionRating = value; }
        public virtual float? CumulativeAttentionRating { get => cumulativeAttentionRating; set => cumulativeAttentionRating = value; }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        public virtual void Update()
        {
            // Every frame get a local copy of attention values.
            // In classes inheriting from this we need to call base.Update(); to get these values, or just call them ourselves.
            AttentionRating = AttentionTracker.getFocusValue;
            CumulativeAttentionRating = AttentionTracker.getCumulativeFocusValue;
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

        // Map a value from one range to another.
        public virtual float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
