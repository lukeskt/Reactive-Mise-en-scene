using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Remes
{
    public class EnableListOfObjects : MonoBehaviour
    {
        public GameObject[] objectsToActivate;

        // Start is called before the first frame update
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                // Activate all specified objects (i.e. empty containers for reactive elements)
                foreach (var obj in objectsToActivate)
                {
                    obj.SetActive(true);
                }
            }
            // Remove this component - but keep trigger in case useful later?
            Destroy(this);
        }
    }
}