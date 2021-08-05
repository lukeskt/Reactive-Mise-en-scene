using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactiveMedia
{
    public class DestroyTendencyObjects : MonoBehaviour
    {
        public OldLoaderMode loaderMode = OldLoaderMode.Tendency;
        public RequestType requestType;
        public Tendencies tendencyListToDestroy;
        //public TendencyAlgorithm tendencyDecision;
        public Locales localeToParse;
        AttentionDataManager DataMgr;

        [System.Serializable]
        public class tendencyPrefabs
        {
            public Tendencies tendency;
            public List<GameObject> TendencyPrefabs;
        }

        [System.Serializable]
        public class TendencyPrefabList
        {
            public List<tendencyPrefabs> ListOfTendencyLists;
        }

        public TendencyPrefabList TendencyObjects = new TendencyPrefabList();

        public void DestroyObjects()
        {
            switch (loaderMode)
            {
                case OldLoaderMode.Preset:
                    // maybe more sophisticated behaviour? but this is just a gating mechanism.
                    // quick test
                    var tendencyCheck = TendencyObjects.ListOfTendencyLists.First(tendencylist => tendencylist.tendency == tendencyListToDestroy);
                    if (tendencyCheck.Equals(tendencyListToDestroy))
                    {
                        foreach (var obj in tendencyCheck.TendencyPrefabs)
                        {
                            Destroy(obj);
                        }
                    }                  
                    break;
                case OldLoaderMode.Random:
                    int randList = Random.Range(0, TendencyObjects.ListOfTendencyLists.Count);
                    int randObj = Random.Range(0, TendencyObjects.ListOfTendencyLists[randList].TendencyPrefabs.Count);
                    Destroy(TendencyObjects.ListOfTendencyLists[randList].TendencyPrefabs[randObj]);
                    break;
                case OldLoaderMode.Tendency:
                    DataMgr = FindObjectOfType<AttentionDataManager>();
                    Dictionary<Tendencies, double> TendenciesFromDataMgr = new Dictionary<Tendencies, double>();
                    Tendencies MaxKey;
                    switch (requestType)
                    {
                        // maybe want inverse/min value options here too...?
                        case RequestType.Locale:
                            TendenciesFromDataMgr = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, localeToParse);
                            MaxKey = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                            break;
                        case RequestType.Global:
                            TendenciesFromDataMgr = DataMgr.GetGlobalTendency(DataMgr.attentionObjects);
                            MaxKey = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                            break;
                        default:
                            MaxKey = Tendencies.Neutral; // set a default just in case.
                            break;
                    }
                    foreach (var tendencyList in TendencyObjects.ListOfTendencyLists)
                    {
                        if (tendencyList.tendency == MaxKey)
                        {
                            foreach (var obj in tendencyList.TendencyPrefabs)
                            {
                                Destroy(obj);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}