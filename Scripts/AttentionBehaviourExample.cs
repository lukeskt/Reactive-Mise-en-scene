using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remes
{
    public class AttentionBehaviourExample : AttentionBehaviour
    {
        public override float AttentionRating { get => base.AttentionRating; set => base.AttentionRating = value; }
        public override float CumulativeAttentionRating { get => base.CumulativeAttentionRating; set => base.CumulativeAttentionRating = value; }

        public override Dictionary<float, bool> CheckAgainstMultipleThresholds(float rating, params float[] thresholds)
        {
            return base.CheckAgainstMultipleThresholds(rating, thresholds);
        }

        public override bool CheckRatingAgainstThreshold(float rating, float threshold)
        {
            return base.CheckRatingAgainstThreshold(rating, threshold);
        }

        public override float MapValue(float inputValue, float fromMin, float fromMax, float toMin, float toMax)
        {
            return base.MapValue(inputValue, fromMin, fromMax, toMin, toMax);
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            float[] threshes = { 1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f, 0.05f, 0f };
            var threshChecks = CheckAgainstMultipleThresholds(AttentionRating, threshes);
            foreach (var item in threshChecks)
            {
                if (item.Value == true)
                {
                    print($"Attention rating {AttentionRating} is higher than {item.Key}!");
                }
            }
        }
    }
}
