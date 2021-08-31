using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class DestroyObjects : MonoBehaviour
    {
        public ReactiveMesSettings RMSettings;

        public ReactiveMesSettings.TendencyAlgorithm tendencyAlgorithm = ReactiveMesSettings.TendencyAlgorithm.MaxValue;
        public ReactiveMesSettings.RequestType requestType;
        public string tendencyListToDestroy;
        [HideInInspector] public int tendencyIndex = 0;
        //public TendencyAlgorithm tendencyDecision;
        public string localeRequest;
        [HideInInspector] public int localeIndex = 0;

        private ReactiveMesDataManager DataMgr;

        [System.Serializable]
        public class tendencyPrefabs
        {
            public string tendency;
            [HideInInspector] public int tendencyIndex = 0;
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
            Dictionary<string, double> TendenciesFromDataMgr = new Dictionary<string, double>();
            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
            string TendencyToDestroy;
            switch (requestType)
            {
                case ReactiveMesSettings.RequestType.Global:
                    TendenciesFromDataMgr = DataMgr.GetGlobalTendency(DataMgr.attentionObjects);
                    break;
                case ReactiveMesSettings.RequestType.Locale:
                    TendenciesFromDataMgr = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, localeRequest);
                    break;
                default:
                    break;
            }

            switch (tendencyAlgorithm)
            {
                case ReactiveMesSettings.TendencyAlgorithm.MaxValue:
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
                case ReactiveMesSettings.TendencyAlgorithm.MinValue:
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
                case ReactiveMesSettings.TendencyAlgorithm.Proportional:
                    NotImpl();
                    break;
                case ReactiveMesSettings.TendencyAlgorithm.InverseProportion:
                    NotImpl();
                    break;
                case ReactiveMesSettings.TendencyAlgorithm.CompetitorDistribution:
                    NotImpl();
                    break;
                case ReactiveMesSettings.TendencyAlgorithm.Preset:
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
                case ReactiveMesSettings.TendencyAlgorithm.Random:
                    int randList = Random.Range(0, TendencyObjects.ListOfTendencyLists.Count);
                    int randObj = Random.Range(0, TendencyObjects.ListOfTendencyLists[randList].TendencyPrefabs.Count);
                    Destroy(TendencyObjects.ListOfTendencyLists[randList].TendencyPrefabs[randObj]);
                    break;
                default:
                    goto case ReactiveMesSettings.TendencyAlgorithm.Preset;
            }
        }

        private void NotImpl()
        {
            throw new System.NotImplementedException();
        }
    }
}