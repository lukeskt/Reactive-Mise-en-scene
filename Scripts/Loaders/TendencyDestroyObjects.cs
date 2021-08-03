using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactiveMedia
{
    public class TendencyDestroyObjects : MonoBehaviour
    {
        public OldLoaderMode loaderMode = OldLoaderMode.Tendency;
        public RequestType requestType;
        public int presetObjectListToDestroy = 0;
        //public TendencyAlgorithm tendencyDecision;
        public Locales localeToParse;
        AttentionDataManager DataMgr;

        [System.Serializable]
        public class tendencyPrefabs
        {
            public List<GameObject> TendencyPrefabs;
        }

        [System.Serializable]
        public class TendencyPrefabList
        {
            public List<tendencyPrefabs> ListOfTendencyLists;
        }

        public TendencyPrefabList TendencyObjects = new TendencyPrefabList();

        public void DestroyTendencyObjects()
        {
            switch (loaderMode)
            {
                case OldLoaderMode.Preset:
                    // maybe more sophisticated behaviour? but this is just a gating mechanism.
                    // quick dumb test
                    Destroy(TendencyObjects.ListOfTendencyLists[0].TendencyPrefabs[presetObjectListToDestroy]);
                    break;
                case OldLoaderMode.Random:
                    int randList = Random.Range(0, TendencyObjects.ListOfTendencyLists.Count);
                    int randObj = Random.Range(0, TendencyObjects.ListOfTendencyLists[randList].TendencyPrefabs.Count);
                    Destroy(TendencyObjects.ListOfTendencyLists[randList].TendencyPrefabs[randObj]);
                    break;
                case OldLoaderMode.Tendency:
                    DataMgr = FindObjectOfType<AttentionDataManager>();
                    switch (requestType)
                    {
                        // maybe want inverse/min value options here too...?
                        case RequestType.Locale:
                            var LocaleTendencies = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, localeToParse);
                            var LocaleMaxKey = LocaleTendencies.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                            switch (LocaleMaxKey)
                            {
                                case Tendencies.Neutral:
                                    foreach (var obj in TendencyObjects.ListOfTendencyLists[0].TendencyPrefabs)
                                    {
                                        Destroy(obj);
                                    }
                                    break;
                                case Tendencies.Spy:
                                    foreach (var obj in TendencyObjects.ListOfTendencyLists[1].TendencyPrefabs)
                                    {
                                        Destroy(obj);
                                    }
                                    break;
                                case Tendencies.Terrorist:
                                    foreach (var obj in TendencyObjects.ListOfTendencyLists[2].TendencyPrefabs)
                                    {
                                        Destroy(obj);
                                    }
                                    break;
                                //case Tendencies.Resistance:
                                //    foreach (var obj in TendencyObjects.ListOfTendencyLists[3].TendencyPrefabs)
                                //    {
                                //        Destroy(obj);
                                //    }
                                //    break;
                                //case Tendencies.Oneirica:
                                //    // should be 4 here, testing...?
                                //    foreach (var obj in TendencyObjects.ListOfTendencyLists[3].TendencyPrefabs)
                                //    {
                                //        Destroy(obj);
                                //    }
                                //    break;
                                case Tendencies.Auteur:
                                    // normally [5] here, testing...?
                                    foreach (var obj in TendencyObjects.ListOfTendencyLists[3].TendencyPrefabs)
                                    {
                                        Destroy(obj);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case RequestType.Global:
                            var GlobalTendencies = DataMgr.GetGlobalTendency(DataMgr.attentionObjects);
                            var GlobalMaxKey = GlobalTendencies.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                            switch (GlobalMaxKey)
                            {
                                case Tendencies.Neutral:
                                    foreach (var obj in TendencyObjects.ListOfTendencyLists[0].TendencyPrefabs)
                                    {
                                        Destroy(obj);
                                    }
                                    break;
                                case Tendencies.Spy:
                                    foreach (var obj in TendencyObjects.ListOfTendencyLists[1].TendencyPrefabs)
                                    {
                                        Destroy(obj);
                                    }
                                    break;
                                case Tendencies.Terrorist:
                                    foreach (var obj in TendencyObjects.ListOfTendencyLists[2].TendencyPrefabs)
                                    {
                                        Destroy(obj);
                                    }
                                    break;
                                //case Tendencies.Resistance:
                                //    foreach (var obj in TendencyObjects.ListOfTendencyLists[3].TendencyPrefabs)
                                //    {
                                //        Destroy(obj);
                                //    }
                                //    break;
                                //case Tendencies.Oneirica:
                                //    // should be 4 here, testing...?
                                //    foreach (var obj in TendencyObjects.ListOfTendencyLists[3].TendencyPrefabs)
                                //    {
                                //        Destroy(obj);
                                //    }
                                //    break;
                                case Tendencies.Auteur:
                                    // normally [5] here, testing...?
                                    foreach (var obj in TendencyObjects.ListOfTendencyLists[3].TendencyPrefabs)
                                    {
                                        Destroy(obj);
                                    }
                                    break;
                                default:
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}