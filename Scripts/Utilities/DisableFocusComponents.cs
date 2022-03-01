using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class DisableFocusComponents : MonoBehaviour
    {
        public GameObject[] objectsToRemoveComponentFrom;

        // Start is called before the first frame update
        void Start()
        {
            foreach (var obj in objectsToRemoveComponentFrom)
            {
                //Destroy(obj.GetComponent<FocusMeasures>());
                //Destroy(obj.GetComponent<Focus>());
                obj.GetComponent<FocusTimeTracking>().enabled = false;
                obj.GetComponent<Focus>().enabled = false;
            }
        }
    }
}
