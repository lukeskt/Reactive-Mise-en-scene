using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class EnableObjectHierarchyRecursively : MonoBehaviour
    {
        // NOTE: Put this on the top level empty gameobject that contains the basic geometry you want to load for a location.
        // Simply enables the hierarchy recursively down from the object to which it is attached.
        // Maybe instantiate from prefab or load via scene instead or as well as?
        // Doesn't do tendency-based stuff. Maybe iterate to add that later?
        void Start()
        {
            SetActiveRecursively(transform, true);
        }

        private void SetActiveRecursively(Transform parent, bool active)
        {
            parent.gameObject.SetActive(active);
            foreach (Transform child in parent)
            {
                SetActiveRecursively(child, active);
            }
        }
    }
}