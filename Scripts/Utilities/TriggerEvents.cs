using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Remes
{
    public class TriggerEvents : MonoBehaviour
    {
        public UnityEvent triggerEnter;
        public UnityEvent triggerStay;
        public UnityEvent triggerExit;

        private void OnTriggerEnter(Collider other)
        {
            triggerEnter.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            triggerStay.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            triggerExit.Invoke();
        }
    }
}
