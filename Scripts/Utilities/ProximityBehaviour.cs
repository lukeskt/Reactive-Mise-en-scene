using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Remes
{
    [RequireComponent(typeof(SphereCollider))]
    public class ProximityBehaviour : MonoBehaviour
    {
        public SphereCollider triggerCollider;
        private float distance;
        private float heading;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<SphereCollider>().isTrigger = true;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.tag == "Player" && GetLineOfSight(other))
            {
                distance = GetDistance(other);
                heading = GetHeading(other);
                print($"{other.name} has distance of {distance} and heading of {heading}");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            
        }

        private bool GetLineOfSight(Collider other)
        {
            if (Physics.Linecast(other.transform.position, transform.position, out RaycastHit hit, 1 << gameObject.layer, QueryTriggerInteraction.Ignore)
                && hit.collider.gameObject.name == this.gameObject.name) {
                Debug.DrawLine(transform.position, other.transform.position, Color.blue);
                return true;
            }
            else
            {
                Debug.DrawLine(transform.position, other.transform.position, Color.magenta);
                return false;
            }
        }

        private float GetDistance(Collider other)
        {
            return Vector3.Distance(transform.position, other.transform.position);
        }

        private float GetHeading(Collider other)
        {
            Vector3 targetDir = other.transform.position - transform.position;
            Vector3 forward = transform.forward;
            return Vector3.SignedAngle(targetDir, forward, Vector3.up);
        }
    }
}
