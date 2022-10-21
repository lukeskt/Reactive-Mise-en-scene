using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remes
{
    public class AttentionBehaviourExample : AttentionBehaviour
    {
        // For AttentionBehaviour derived classes make sure you set public override on Upate,
        // and first call base.Update(); in it to get current attention and cumulative attention values.
        // You can then reference these as AttentionRating and CumulativeAttentionRating respectively.
        public override void Update()
        {
            base.Update();
            float[] threshes = { 1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f, 0.05f, 0f };
            var threshChecks = CheckAgainstMultipleThresholds((float)AttentionRating, threshes);
            foreach (var item in threshChecks)
            {
                if (item.Value == true)
                {
                    print($"Attention Rating {AttentionRating} is higher than {item.Key}!");
                    break;
                }
                else
                {
                    print($"Attention Rating {AttentionRating} is 0 or less!");
                    break;
                }
            }
            print($"Cumulative Attention is currently at: {CumulativeAttentionRating}");
        }
    }
}
