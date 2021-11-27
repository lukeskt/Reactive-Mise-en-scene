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

        [Tooltip("WARNING EXPERIMENTAL FEATURE!\nIf Continuous is checked, update the tendency-based timeline every frame.")]
        public bool continuous = false;

        public List<GameObject> tendencyObjects;

        // Start is called before the first frame update
        void Start()
        {
            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
            PrefabLoader(localeRequest);
        }

        // Update is called once per frame
        void Update()
        {
            if (continuous)
            {
                PrefabLoader(localeRequest); // change this to the locale locale?
            }
        }

        private void PrefabLoader(string localeRequest)
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
                    break;
            }

            switch (algorithm)
            {
                // TODO: Make the lookups generic with tolist orderby, not aggregate?
                // Maybe make sure the ordering is the same and then do 0, 1, -1, -2?
                // Also need to move the check into datamgr maybe for reuse, and just spawn obj here?
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.StrongestTendency:
                    //TendencyForPrefab = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    var ASortedTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).Reverse().ToList();
                    TendencyForPrefab = ASortedTendencies[0].Key;
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.SecondStrongest:
                    var SortedTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).Reverse().ToList();
                    TendencyForPrefab = SortedTendencies[1].Key;
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.SecondWeakest:
                    var UnsortTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).ToList();
                    TendencyForPrefab = UnsortTendencies[1].Key;
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.WeakestTendency:
                    //TendencyForPrefab = TendenciesFromDataMgr.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    var ResortTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).ToList();
                    TendencyForPrefab = ResortTendencies[0].Key;
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.Random:
                    TendencyForPrefab = tendencyObjects[UnityEngine.Random.Range(0, tendencyObjects.Count)].ToString();
                    break;
                default:
                    goto case ReactiveMesSettings.SingleResultTendencyAlgorithm.StrongestTendency;
            }

            GameObject obj;
            if (tendencyObjects.Find(obj => obj.GetComponent<FocusMeasures>() != null && obj.GetComponent<FocusMeasures>().tendency.Equals(TendencyForPrefab)))
            {
                obj = tendencyObjects.Find(obj => obj.GetComponent<FocusMeasures>().tendency.Equals(TendencyForPrefab));
            }
            else if (tendencyObjects.Find(obj => obj.GetComponent<ReactiveMesObjectTags>() != null && obj.GetComponent<ReactiveMesObjectTags>().tendency.Equals(TendencyForPrefab)))
            {
                obj = tendencyObjects.Find(obj => obj.GetComponent<ReactiveMesObjectTags>().tendency.Equals(TendencyForPrefab));
            }
            else
            {
                obj = null;
            }

            spawnObject(obj, gameObject);
        }



        private void spawnObject(GameObject objectToSpawn, GameObject placementPoint)
        {
            // If placement point contains child then delete that before spawning? Allow for continuous like this?
            Instantiate(objectToSpawn, placementPoint.transform.position, placementPoint.transform.rotation, placementPoint.transform);
            //Destroy(placementPoint);
        }
    }
}
