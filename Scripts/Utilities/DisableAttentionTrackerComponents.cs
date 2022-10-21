using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remes
{
    public class DisableAttentionTrackerComponents : MonoBehaviour
    {
        public GameObject[] objectsToRemoveComponentFrom;

        // Start is called before the first frame update
        void Start()
        {
            foreach (var obj in objectsToRemoveComponentFrom)
            {
                obj.GetComponent<AttentionTracker>().enabled = false;
            }
        }
    }
}
