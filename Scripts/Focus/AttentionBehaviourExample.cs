using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class AttentionBehaviourExample : AttentionBehaviour
    {
        public override float AttentionRating { get => base.AttentionRating; set => base.AttentionRating = value; }

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
            AttentionRating = focusObj.getFocusValue;
            var threshChecks = CheckAgainstMultipleThresholds(AttentionRating, AttentionRating);
            foreach (var item in threshChecks)
            {
                if (item.Value == true)
                {
                    print($"Attention rating is higher than {item.Key}!");
                }
            }
        }
    }
}
