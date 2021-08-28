using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class DestroyObjects : MonoBehaviour
    {
        public TendencyAlgorithm tendencyAlgorithm = TendencyAlgorithm.MaxValue;
        public RequestType requestType;
        public Tendencies tendencyListToDestroy;
        //public TendencyAlgorithm tendencyDecision;
        public Locales localeToParse;
        ReactiveMesDataManager DataMgr;

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

        public void DoDestroyObjects()
        {
            Dictionary<Tendencies, double> TendenciesFromDataMgr = new Dictionary<Tendencies, double>();
            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
            Tendencies TendencyToDestroy;
            switch (requestType)
            {
                case RequestType.Global:
                    TendenciesFromDataMgr = DataMgr.GetGlobalTendency(DataMgr.attentionObjects);
                    break;
                case RequestType.Locale:
                    TendenciesFromDataMgr = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, localeToParse);
                    break;
                default:
                    break;
            }

            switch (tendencyAlgorithm)
            {
                case TendencyAlgorithm.MaxValue:
                    TendencyToDestroy = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    foreach (var tendencyList in TendencyObjects.ListOfTendencyLists)
                    {
                        if (tendencyList.tendency == TendencyToDestroy)
                        {
                            foreach (var obj in tendencyList.TendencyPrefabs)
                            {
                                Destroy(obj);
                            }
                        }
                    }
                    break;
                case TendencyAlgorithm.MinValue:
                    TendencyToDestroy = TendenciesFromDataMgr.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    foreach (var tendencyList in TendencyObjects.ListOfTendencyLists)
                    {
                        if (tendencyList.tendency == TendencyToDestroy)
                        {
                            foreach (var obj in tendencyList.TendencyPrefabs)
                            {
                                Destroy(obj);
                            }
                        }
                    }
                    break;
                case TendencyAlgorithm.Proportional:
                    NotImpl();
                    break;
                case TendencyAlgorithm.InverseProportion:
                    NotImpl();
                    break;
                case TendencyAlgorithm.CompetitorDistribution:
                    NotImpl();
                    break;
                case TendencyAlgorithm.Preset:
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
                case TendencyAlgorithm.Random:
                    int randList = Random.Range(0, TendencyObjects.ListOfTendencyLists.Count);
                    int randObj = Random.Range(0, TendencyObjects.ListOfTendencyLists[randList].TendencyPrefabs.Count);
                    Destroy(TendencyObjects.ListOfTendencyLists[randList].TendencyPrefabs[randObj]);
                    break;
                default:
                    goto case TendencyAlgorithm.Preset;
            }
        }

        private void NotImpl()
        {
            throw new System.NotImplementedException();
        }
    }
}