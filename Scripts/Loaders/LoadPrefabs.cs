using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactiveMiseEnScene
{
    public class LoadPrefabs : MonoBehaviour
    {
        public ReactiveMesSettings RMSettings;

        [Tooltip("Select which algorithm to use to decide which object to load at each placement point.")]
        [SerializeField] public ReactiveMesSettings.MultiResultTendencyAlgorithm algorithm;
        [Tooltip("If using Preset algorithm, use this to specify the tendency to load objects for.")]
        [SerializeField] public string presetTendency;
        [HideInInspector] public int tendencyIndex = 0; // for custom editor
        [Tooltip("Global: Get global attention rating. Locale: Get attention rating of specified locale.")]
        [SerializeField] public ReactiveMesSettings.RequestType requestType;
        [Tooltip("If using Locale request type, specify locale from which to get attention rating.")]
        [SerializeField] public string localeRequest;
        [HideInInspector] public int localeIndex = 0; // for custom editor

        [System.Serializable]
        public class TendencyPlacements
        {
            [HideInInspector] public string Name = "Placement Point";
            public GameObject placementPoint;
            public List<GameObject> tendencyObjects;
        }

        [System.Serializable]
        public class ListOfTendencyPlacements
        {
            public List<TendencyPlacements> tendencyPlacements;
        }

        [SerializeField] public ListOfTendencyPlacements listOfTendencyPlacements = new ListOfTendencyPlacements();

        private double amortized = 0;

        // Start is called before the first frame update
        void Start()
        {
            ReactiveMesDataManager DataMgr = FindObjectOfType<ReactiveMesDataManager>();
                  
            Dictionary<string, double> tendencyAttentionRatings;
            List<KeyValuePair<string, double>> orderedTendencyAttentionRatings;

            switch (requestType)
                    {
                        case ReactiveMesSettings.RequestType.Global:
                            tendencyAttentionRatings = DataMgr.GetGlobalTendency(DataMgr.attentionObjects);
                            break;
                        case ReactiveMesSettings.RequestType.Locale:
                            tendencyAttentionRatings = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, localeRequest);
                            break;
                        default:
                            goto case ReactiveMesSettings.RequestType.Global;
                    }

            orderedTendencyAttentionRatings = tendencyAttentionRatings.ToList();
            orderedTendencyAttentionRatings.Sort((x, y) => x.Value.CompareTo(y.Value));
            orderedTendencyAttentionRatings.Reverse(); // = descending values.

            switch (algorithm)
            {
                case ReactiveMesSettings.MultiResultTendencyAlgorithm.MaxValue:
                    StartCoroutine(MaxValueLoader(orderedTendencyAttentionRatings));
                    break;
                case ReactiveMesSettings.MultiResultTendencyAlgorithm.MinValue:
                    StartCoroutine(MinValueLoader(orderedTendencyAttentionRatings));
                    break;
                case ReactiveMesSettings.MultiResultTendencyAlgorithm.Proportional:
                    //ProportionalLoader(orderedTendencyAttentionRatings);
                    StartCoroutine(ProportionalLoader(orderedTendencyAttentionRatings));
                    break;
                case ReactiveMesSettings.MultiResultTendencyAlgorithm.InverseProportion:
                    StartCoroutine(InverseProportionalLoader(orderedTendencyAttentionRatings));
                    break;
                //case ReactiveMesSettings.MultiResultTendencyAlgorithm.CompetitorDistribution:
                //    StartCoroutine(CompetitorDistributionLoader(orderedTendencyAttentionRatings));
                //    break;
                case ReactiveMesSettings.MultiResultTendencyAlgorithm.Preset:
                    StartCoroutine(PresetLoader());
                    break;
                case ReactiveMesSettings.MultiResultTendencyAlgorithm.Random:
                    StartCoroutine(RandomLoader());
                    break;
                default:
                    break;
            }
        }

        private IEnumerator MaxValueLoader(List<KeyValuePair<string, double>> orderedTendencyAttentionRatings)
        {
            foreach (var tendencyPlacement in listOfTendencyPlacements.tendencyPlacements)
            {
                // TODO: check throughout to try obj.GetComponent<FocusMeasures>() then if fails try GetComponent<ObjectLocaleTendencyTags>()!
                spawnObject(
                    tendencyPlacement.tendencyObjects.Find(obj => obj.GetComponent<ObjectLocaleTendencyTags>().tendency.Equals(orderedTendencyAttentionRatings.Aggregate((l, r) => l.Value > r.Value ? l : r).Key)),
                    tendencyPlacement.placementPoint
                    );
            }
            yield return null;
        }

        private IEnumerator MinValueLoader(List<KeyValuePair<string, double>> orderedTendencyAttentionRatings)
        {
            

            foreach (var tendencyPlacement in listOfTendencyPlacements.tendencyPlacements)
            {
                spawnObject(
                    tendencyPlacement.tendencyObjects.Find(obj => obj.GetComponent<ObjectLocaleTendencyTags>().tendency.Equals(orderedTendencyAttentionRatings.Aggregate((l, r) => l.Value < r.Value ? l : r).Key)),
                    tendencyPlacement.placementPoint
                    );
            }
            yield return null;
        }

        private List<KeyValuePair<string, int>> MapOrderedRatings(List<KeyValuePair<string, double>> orderedTendencyAttentionRatings)
        {
            double tendencySum = orderedTendencyAttentionRatings.Sum(tendency => tendency.Value);
            int placementPointsCount = listOfTendencyPlacements.tendencyPlacements.Count();
            Dictionary<string, int> mappedTendencyAttentionRatings = new Dictionary<string, int>();
            foreach (var tendency in orderedTendencyAttentionRatings)
            {
                var mappedRating = mapTendencyToSpawnListLength(tendency.Value, tendencySum, placementPointsCount);
                mappedTendencyAttentionRatings.Add(tendency.Key, mappedRating);
            }
            var orderedMappedRatings = mappedTendencyAttentionRatings.ToList();
            orderedMappedRatings.Sort((x, y) => x.Value.CompareTo(y.Value));
            orderedMappedRatings.Reverse();
            return orderedMappedRatings;
        }

        private static List<string> BuildListOfTendenciesToMapForPlacements(List<KeyValuePair<string, int>> orderedMappedRatings)
        {
            List<string> tendencyList = new List<string>();
            foreach (var rating in orderedMappedRatings)
            {
                for (int i = 0; i < rating.Value; i++)
                {
                    tendencyList.Add(rating.Key);
                }
            }

            return tendencyList;
        }

        private void SpawnObjectsBasedOnTendenciesAtPlacementPoints(List<string> tendencyList)
        {
            // there may be an index out of range issue down here - consider how to handle - trycatch?
            for (int i = 0; i < listOfTendencyPlacements.tendencyPlacements.Count; i++)
            {
                spawnObject(
                    listOfTendencyPlacements.tendencyPlacements[i].tendencyObjects.Find(obj => obj.GetComponent<ObjectLocaleTendencyTags>().tendency.Equals(tendencyList[i])),
                    listOfTendencyPlacements.tendencyPlacements[i].placementPoint
                    );
            }
        }

        private IEnumerator ProportionalLoader(List<KeyValuePair<string, double>> orderedTendencyAttentionRatings)
        {
            List<KeyValuePair<string, int>> orderedMappedRatings = MapOrderedRatings(orderedTendencyAttentionRatings);
            // this seems like 4th impl? - it works sort of. above could be put into a separate method?
            List<string> tendencyList = BuildListOfTendenciesToMapForPlacements(orderedMappedRatings);
            SpawnObjectsBasedOnTendenciesAtPlacementPoints(tendencyList);

            yield return null;
        }

        private IEnumerator InverseProportionalLoader(List<KeyValuePair<string, double>> orderedTendencyAttentionRatings)
        {
            List<KeyValuePair<string, int>> orderedMappedRatings = MapOrderedRatings(orderedTendencyAttentionRatings);

            // Inversion Logic
            // the smallest tendency gets the max tendency's placement points, 2nd smallest = 2nd biggest, etc.
            List<string> reverseTendencies = (from kvp in orderedMappedRatings select kvp.Key).Distinct().Reverse().ToList();
            List<int> ratingValues = (from kvp in orderedMappedRatings select kvp.Value).ToList();
            List<KeyValuePair<string, int>> inverseProportionalRatings = reverseTendencies.Zip(ratingValues, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v).ToList();

            List<string> tendencyList = BuildListOfTendenciesToMapForPlacements(orderedMappedRatings);
            SpawnObjectsBasedOnTendenciesAtPlacementPoints(tendencyList);

            yield return null;
        }

        private IEnumerator CompetitorDistributionLoader(List<KeyValuePair<string, double>> orderedTendencyAttentionRatings)
        {
            throw new System.NotImplementedException();
            ////print(String.Join(", ", orderedTendencyAttentionRatings));
            //List<KeyValuePair<Tendencies, int>> orderedMappedRatings = MapOrderedRatings(orderedTendencyAttentionRatings);
            ////print(String.Join(", ", orderedMappedRatings));

            //// Max tendency gets the remainder placement points
            //// the placement points it would get are distributed amongst other tendencies.
            //// e.g. if 6/10 max, 6 points evenly divided amongst other tendencies, max gets remainder?
            //// TODO: THIS CODE DOESN'T WORK PROPERLY!
            //List<Tendencies> distroTendencies = new List<Tendencies>();
            //int maxVal = orderedMappedRatings[0].Value;
            //int distroMaxVal = maxVal / (orderedMappedRatings.Count - 1);
            //int remainderVal = 0;
            //for (int i = 1; i < orderedMappedRatings.Count; i++)
            //{
            //    remainderVal += orderedMappedRatings[i].Value;
            //}
            //List<int> distroValues = new List<int>();
            //distroValues.Add(remainderVal);
            //for (int i = 1; i < orderedMappedRatings.Count; i++)
            //{
            //    distroValues.Add(distroMaxVal);
            //}
            //List<KeyValuePair<Tendencies, int>> competitorDistroRatings = distroTendencies.Zip(distroValues, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v).ToList();
            ////print("COMPETITOR DISTRIBUTION!");
            ////print(String.Join(", ", competitorDistroRatings));

            //List<Tendencies> tendencyList = new List<Tendencies>();
            //foreach (var rating in competitorDistroRatings)
            //{
            //    for (int i = 0; i < rating.Value; i++)
            //    {
            //        tendencyList.Add(rating.Key);
            //    }
            //}

            //// spawn objects based on above.
            //for (int i = 0; i < listOfTendencyPlacements.tendencyPlacements.Count; i++)
            //{
            //    spawnObject(
            //        listOfTendencyPlacements.tendencyPlacements[i].tendencyObjects.Find(obj => obj.GetComponent<FocusMeasures>().tendency.Equals(tendencyList[i])),
            //        listOfTendencyPlacements.tendencyPlacements[i].placementPoint
            //        );
            //}

            //yield return null;
        }

        private IEnumerator PresetLoader()
        {
            // TODO: Change this from a single tendency to spawn an authored list of objects.
            foreach (var tendencyPlacement in listOfTendencyPlacements.tendencyPlacements)
            {
                spawnObject(
                    tendencyPlacement.tendencyObjects.Find(obj => obj.GetComponent<ObjectLocaleTendencyTags>().tendency.Equals(presetTendency)),
                    tendencyPlacement.placementPoint
                    );
            }
            yield return null;
        }

        private IEnumerator RandomLoader()
        {
            foreach (var tendencyPlacement in listOfTendencyPlacements.tendencyPlacements)
            {
                spawnObject(
                    tendencyPlacement.tendencyObjects[UnityEngine.Random.Range(0, tendencyPlacement.tendencyObjects.Count)],
                    tendencyPlacement.placementPoint
                    );
            }
            yield return null;
        }

        private void spawnObject(GameObject objectToSpawn, GameObject placementPoint)
        {
            Instantiate(objectToSpawn, placementPoint.transform.position, placementPoint.transform.rotation, placementPoint.transform.parent);
            Destroy(placementPoint);
        }

        private int mapTendencyToSpawnListLength(double tendency, double sumOfTendencies, int objectListLength)
        {
            int objQuantity = (int)Math.Round(tendency / sumOfTendencies * objectListLength);
            //int objQuantity = (int)Math.Floor(tendency / sumOfTendencies * objectListLength);
            return objQuantity;
        }

        // Per https://stackoverflow.com/questions/27330331/how-do-i-optimally-distribute-values-over-an-array-of-percentages
        private int bresenhamMapping(double tendency, double sumOfTendencies)
        {
            double real;
            int natural;

            real = tendency * sumOfTendencies + amortized;
            natural = (int)Math.Floor(real);
            amortized = real - natural;

            return natural;
        }

        private void OnDrawGizmos()
        {
            // anything here?
        }
    }
}