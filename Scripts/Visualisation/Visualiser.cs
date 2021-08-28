using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace ReactiveMiseEnScene
{
    public class Visualiser : MonoBehaviour
    {
        public List<GameObject> localeNodes = new List<GameObject>();
        public List<Color32> nodeColors = new List<Color32>();
        public List<Color32> edgeColors = new List<Color32>();

        private ReactiveMesDataManager DataMgr;

        // Start is called before the first frame update
        void Start()
        {
            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
        }

        private void OnDrawGizmos()
        {
            // testing.
            GameObject oldNode = null;
            foreach (var node in localeNodes)
            {
                //Gizmos.color = nodeColors[localeNodes.IndexOf(node)];
                //Gizmos.DrawCube(node.transform.position + new Vector3 (0, 5, 0), new Vector3(4, 1, 4));
                Handles.color = nodeColors[localeNodes.IndexOf(node)];
                Handles.DrawSolidDisc(node.transform.position + new Vector3(0, 10, 0), new Vector3(0, 1, 0), 2f);
                string LocaleName = node.GetComponent<LocaleInfo>().locale.ToString();
                string LocaleTendency = node.GetComponent<LocaleInfo>().localeTendency.ToString();
                string LocaleTendencyRating = node.GetComponent<LocaleInfo>().localeTendencyRating.ToString();
                Handles.Label(node.transform.position + new Vector3 (0, 11f, -0.5f), $"Locale: {LocaleName}\nTendency: {LocaleTendency}\nRating: {LocaleTendencyRating}");
                //Gizmos.color = edgeColors[localeNodes.IndexOf(node)];
                Handles.color = edgeColors[localeNodes.IndexOf(node)];
                if (oldNode)
                {
                    //Gizmos.DrawLine(oldNode.transform.position + new Vector3(0, 5, 0), node.transform.position + new Vector3(0, 5, 0));
                    Handles.DrawLine(oldNode.transform.position + new Vector3(0, 10, 0), node.transform.position + new Vector3(0, 10, 0), 10f);
                }
                oldNode = node;
            }

            /** TODO: Size and color of nodes by tendency 
             * Draw lines between all locales
             * Set line colours by tendency if the areas connect?
             * Handles.DrawPolyline
             */
        }
    }
}
#endif
