using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class LoadPrefab : MonoBehaviour
    {
        public ReactiveMesSettings RMSettings;
        ReactiveMesDataManager DataMgr;

        public ReactiveMesSettings.SingleResultTendencyAlgorithm algorithm;
        public ReactiveMesSettings.RequestType requestType;

        [HideInInspector] public int tendencyIndex = 0; // for custom editor

        public string localeRequest;
        [HideInInspector] public int localeIndex = 0; // for custom editor

        [Header("Modes")]
        [Tooltip("If enabled any child objects of this object will be deleted before loading the tendency object. Useful if you want to load and update the object at this position more than once.")]
        public bool replaceObject = false;
        [Tooltip("If enabled, loads the relevant tendency object when this component is enabled.")]
        public bool loadOnStart = false;

        [Header("Tendency Objects")]
        public List<GameObject> tendencyObjects;

        // Start is called before the first frame update
        void Start()
        {
            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
            if (loadOnStart) PrefabLoader(); // localeRequest);
        }

        public void PrefabLoader() //string localeRequest)
        {
            // Move this up into the datamgr?
            Dictionary<string, double> TendenciesFromDataMgr = new Dictionary<string, double>();
            string TendencyForPrefab;
            switch (requestType)
            {
                case ReactiveMesSettings.RequestType.Global:
                    TendenciesFromDataMgr = DataMgr.GetGlobalTendency(DataMgr.reactiveObjects);
                    break;
                case ReactiveMesSettings.RequestType.Locale:
                    TendenciesFromDataMgr = DataMgr.GetLocaleTendency(DataMgr.reactiveObjects, localeRequest);
                    break;
                default:
                    goto case ReactiveMesSettings.RequestType.Global;
            }

            List<KeyValuePair<string, double>> StrongestFirstTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).Reverse().ToList();
            List<KeyValuePair<string, double>> WeakestFirstTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).ToList();
            switch (algorithm)
            {
                // Also need to move the check into datamgr maybe for reuse, and just spawn obj here?
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.StrongestTendency:
                    //TendencyForPrefab = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    TendencyForPrefab = StrongestFirstTendencies[0].Key;
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.SecondStrongest:
                    TendencyForPrefab = StrongestFirstTendencies[1].Key;
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.SecondWeakest:
                    TendencyForPrefab = WeakestFirstTendencies[1].Key;
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.WeakestTendency:
                    //TendencyForPrefab = TendenciesFromDataMgr.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    TendencyForPrefab = WeakestFirstTendencies[0].Key;
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.Random:
                    TendencyForPrefab = tendencyObjects[UnityEngine.Random.Range(0, tendencyObjects.Count)].ToString();
                    break;
                default:
                    goto case ReactiveMesSettings.SingleResultTendencyAlgorithm.StrongestTendency;
            }

            GameObject objToSpawn;
            if (tendencyObjects.Find(obj => obj.GetComponent<FocusMeasures>() != null && obj.GetComponent<FocusMeasures>().tendency.Equals(TendencyForPrefab)))
            {
                objToSpawn = tendencyObjects.Find(obj => obj.GetComponent<FocusMeasures>().tendency.Equals(TendencyForPrefab));
            }
            else if (tendencyObjects.Find(obj => obj.GetComponent<RmesObjectTags>() != null && obj.GetComponent<RmesObjectTags>().tendency.Equals(TendencyForPrefab)))
            {
                objToSpawn = tendencyObjects.Find(obj => obj.GetComponent<RmesObjectTags>().tendency.Equals(TendencyForPrefab));
            }
            else
            {
                objToSpawn = null;
            }

            if(replaceObject)
            {
                removePlacementPointChildren();
            }
            spawnObject(objToSpawn);
        }

        private void removePlacementPointChildren ()
        {
            // If placement point contains child then delete that before spawning? Allow for continuous like this?
            if (transform.childCount > 0)
            {
                // we have children!
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        private void spawnObject(GameObject objectToSpawn)
        {
            Instantiate(objectToSpawn, gameObject.transform);
        }
    }
}
