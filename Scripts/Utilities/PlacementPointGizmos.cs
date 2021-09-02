using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class PlacementPointGizmos : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 0.1f);
            Gizmos.DrawIcon(transform.position, "down-arrow.png"); // do a nice icon later instead
        }
    }
}