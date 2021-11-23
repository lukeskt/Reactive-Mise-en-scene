using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class LoadPrefabSingle : MonoBehaviour
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
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.StrongestTendency:
                    TendencyForPrefab = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    spawnObject(tendencyObjects.Find(profile => profile.name.Contains(TendencyForPrefab)), gameObject);
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.SecondStrongest:
                    var SortedTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).Reverse().ToList();
                    spawnObject(tendencyObjects.Find(profile => profile.name.Contains(SortedTendencies[1].Key)), gameObject);
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.SecondWeakest:
                    var UnsortTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).ToList();
                    spawnObject(tendencyObjects.Find(profile => profile.name.Contains(UnsortTendencies[1].Key)), gameObject);
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.WeakestTendency:
                    TendencyForPrefab = TendenciesFromDataMgr.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    spawnObject(tendencyObjects.Find(profile => profile.name.Contains(TendencyForPrefab)), gameObject);
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.Random:
                    spawnObject(tendencyObjects[UnityEngine.Random.Range(0, tendencyObjects.Count)], gameObject);
                    break;
                default:
                    goto case ReactiveMesSettings.SingleResultTendencyAlgorithm.StrongestTendency;
            }
        }

        private void spawnObject(GameObject objectToSpawn, GameObject placementPoint)
        {
            Instantiate(objectToSpawn, placementPoint.transform.position, placementPoint.transform.rotation, placementPoint.transform);
            //Destroy(placementPoint);
        }
    }
}
