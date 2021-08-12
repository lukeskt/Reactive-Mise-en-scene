using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReactiveMedia
{
    public class LoadReactives : MonoBehaviour
    {
        [Tooltip("Select which algorithm to use to decide which object to load at each placement point.")]
        [SerializeField] public TendencyAlgorithm tendencyAlgorithm;
        [Tooltip("If using Preset algorithm, use this to specify the tendency to load objects for.")]
        [SerializeField] public Tendencies presetTendency;
        [Tooltip("Global: Get global attention rating. Locale: Get attention rating of specified locale.")]
        [SerializeField] public RequestType requestType;
        [Tooltip("If using Locale request type, specify locale from which to get attention rating.")]
        [SerializeField] public Locales localeRequest;

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
            AttentionDataManager DataMgr = FindObjectOfType<AttentionDataManager>();
                  
            Dictionary<Tendencies, double> tendencyAttentionRatings;
            List<KeyValuePair<Tendencies, double>> orderedTendencyAttentionRatings;

            switch (requestType)
                    {
                        case RequestType.Global:
                            tendencyAttentionRatings = DataMgr.GetGlobalTendency(DataMgr.attentionObjects);
                            break;
                        case RequestType.Locale:
                            tendencyAttentionRatings = DataMgr.GetLocaleTendency(DataMgr.attentionObjects, localeRequest);
                            break;
                        default:
                            goto case RequestType.Global;
                    }

            orderedTendencyAttentionRatings = tendencyAttentionRatings.ToList();
            orderedTendencyAttentionRatings.Sort((x, y) => x.Value.CompareTo(y.Value));
            orderedTendencyAttentionRatings.Reverse(); // = descending values.

            switch (tendencyAlgorithm)
            {
                case TendencyAlgorithm.MaxValue:
                    StartCoroutine(MaxValueLoader(orderedTendencyAttentionRatings));
                    break;
                case TendencyAlgorithm.MinValue:
                    StartCoroutine(MinValueLoader(orderedTendencyAttentionRatings));
                    break;
                case TendencyAlgorithm.Proportional:
                    //ProportionalLoader(orderedTendencyAttentionRatings);
                    StartCoroutine(ProportionalLoader(orderedTendencyAttentionRatings));
                    break;
                case TendencyAlgorithm.InverseProportion:
                    StartCoroutine(InverseProportionalLoader(orderedTendencyAttentionRatings));
                    break;
                case TendencyAlgorithm.CompetitorDistribution:
                    StartCoroutine(CompetitorDistributionLoader(orderedTendencyAttentionRatings));
                    break;
                case TendencyAlgorithm.Preset:
                    StartCoroutine(PresetLoader());
                    break;
                case TendencyAlgorithm.Random:
                    StartCoroutine(RandomLoader());
                    break;
                default:
                    break;
            }
        }

        private IEnumerator MaxValueLoader(List<KeyValuePair<Tendencies, double>> orderedTendencyAttentionRatings)
        {
            foreach (var tendencyPlacement in listOfTendencyPlacements.tendencyPlacements)
            {
                spawnObject(
                    tendencyPlacement.tendencyObjects.Find(obj => obj.GetComponent<FocusMeasures>().tendency.Equals(orderedTendencyAttentionRatings.Aggregate((l, r) => l.Value > r.Value ? l : r).Key)),
                    tendencyPlacement.placementPoint
                    );
            }
            yield return null;
        }

        private IEnumerator MinValueLoader(List<KeyValuePair<Tendencies, double>> orderedTendencyAttentionRatings)
        {
            foreach (var tendencyPlacement in listOfTendencyPlacements.tendencyPlacements)
            {
                spawnObject(
                    tendencyPlacement.tendencyObjects.Find(obj => obj.GetComponent<FocusMeasures>().tendency.Equals(orderedTendencyAttentionRatings.Aggregate((l, r) => l.Value < r.Value ? l : r).Key)),
                    tendencyPlacement.placementPoint
                    );
            }
            yield return null;
        }

        private IEnumerator ProportionalLoader(List<KeyValuePair<Tendencies, double>> orderedTendencyAttentionRatings)
        {
            //print(String.Join(", ", orderedTendencyAttentionRatings));
            double tendencySum = orderedTendencyAttentionRatings.Sum(tendency => tendency.Value);
            int placementPointsCount = listOfTendencyPlacements.tendencyPlacements.Count();
            Dictionary<Tendencies, int> mappedTendencyAttentionRatings = new Dictionary<Tendencies, int>();
            foreach (var tendency in orderedTendencyAttentionRatings)
            {
                var mappedRating = mapTendencyToSpawnListLength(tendency.Value, tendencySum, placementPointsCount);
                mappedTendencyAttentionRatings.Add(tendency.Key, mappedRating);
            }
            var orderedMappedRatings = mappedTendencyAttentionRatings.ToList();
            orderedMappedRatings.Sort((x, y) => x.Value.CompareTo(y.Value));
            orderedMappedRatings.Reverse();
            //print(String.Join(", ", orderedMappedRatings));

            // this seems like 4th impl? - it works sort of. above could be put into a separate method?
            List<Tendencies> tendencyList = new List<Tendencies>();
            foreach (var rating in orderedMappedRatings)
            {
                for (int i = 0; i < rating.Value; i++)
                {
                    tendencyList.Add(rating.Key);
                }
            }

            //print(String.Join(", ", tendencyList));
            // there may be an index out of range issue down here - consider how to handle - trycatch?
            for (int i = 0; i < listOfTendencyPlacements.tendencyPlacements.Count; i++)
            {
                spawnObject(
                    listOfTendencyPlacements.tendencyPlacements[i].tendencyObjects.Find(obj => obj.GetComponent<FocusMeasures>().tendency.Equals(tendencyList[i])),
                    listOfTendencyPlacements.tendencyPlacements[i].placementPoint
                    );
            }

            yield return null;
        }

        private IEnumerator InverseProportionalLoader(List<KeyValuePair<Tendencies, double>> orderedTendencyAttentionRatings)
        {
            //print(String.Join(", ", orderedTendencyAttentionRatings));
            double tendencySum = orderedTendencyAttentionRatings.Sum(tendency => tendency.Value);
            int placementPointsCount = listOfTendencyPlacements.tendencyPlacements.Count();
            Dictionary<Tendencies, int> mappedTendencyAttentionRatings = new Dictionary<Tendencies, int>();
            foreach (var tendency in orderedTendencyAttentionRatings)
            {
                var mappedRating = mapTendencyToSpawnListLength(tendency.Value, tendencySum, placementPointsCount);
                mappedTendencyAttentionRatings.Add(tendency.Key, mappedRating);
            }
            var orderedMappedRatings = mappedTendencyAttentionRatings.ToList();
            orderedMappedRatings.Sort((x, y) => x.Value.CompareTo(y.Value));
            orderedMappedRatings.Reverse();
            //print(String.Join(", ", orderedMappedRatings));

            // the smallest tendency gets the max tendency's placement points, 2nd smallest = 2nd biggest, etc.
            List<Tendencies> reverseTendencies = (from kvp in orderedMappedRatings select kvp.Key).Distinct().Reverse().ToList();
            List<int> ratingValues = (from kvp in orderedMappedRatings select kvp.Value).ToList();
            List<KeyValuePair<Tendencies, int>> inverseProportionalRatings = reverseTendencies.Zip(ratingValues, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v).ToList();
            //print("INVERTING!");
            //print(String.Join(", ", inverseProportionalRatings));

            List<Tendencies> tendencyList = new List<Tendencies>();
            foreach (var rating in inverseProportionalRatings)
            {
                for (int i = 0; i < rating.Value; i++)
                {
                    tendencyList.Add(rating.Key);
                }
            }

            //print(String.Join(", ", tendencyList));

            for (int i = 0; i < listOfTendencyPlacements.tendencyPlacements.Count; i++)
            {
                spawnObject(
                    listOfTendencyPlacements.tendencyPlacements[i].tendencyObjects.Find(obj => obj.GetComponent<FocusMeasures>().tendency.Equals(tendencyList[i])),
                    listOfTendencyPlacements.tendencyPlacements[i].placementPoint
                    );
            }

            yield return null;
        }

        private IEnumerator CompetitorDistributionLoader(List<KeyValuePair<Tendencies, double>> orderedTendencyAttentionRatings)
        {
            throw new System.NotImplementedException();
            //print(String.Join(", ", orderedTendencyAttentionRatings));
            double tendencySum = orderedTendencyAttentionRatings.Sum(tendency => tendency.Value);
            int placementPointsCount = listOfTendencyPlacements.tendencyPlacements.Count();
            Dictionary<Tendencies, int> mappedTendencyAttentionRatings = new Dictionary<Tendencies, int>();
            foreach (var tendency in orderedTendencyAttentionRatings)
            {
                var mappedRating = mapTendencyToSpawnListLength(tendency.Value, tendencySum, placementPointsCount);
                mappedTendencyAttentionRatings.Add(tendency.Key, mappedRating);
            }
            var orderedMappedRatings = mappedTendencyAttentionRatings.ToList();
            orderedMappedRatings.Sort((x, y) => x.Value.CompareTo(y.Value));
            orderedMappedRatings.Reverse();
            //print(String.Join(", ", orderedMappedRatings));

            // Max tendency gets the remainder placement points
            // the placement points it would get are distributed amongst other tendencies.
            // e.g. if 6/10 max, 6 points evenly divided amongst other tendencies, max gets remainder?
            // TODO: THIS CODE DOESN'T WORK PROPERLY!
            List<Tendencies> distroTendencies = new List<Tendencies>();
            int maxVal = orderedMappedRatings[0].Value;
            int distroMaxVal = maxVal / (orderedMappedRatings.Count - 1);
            int remainderVal = 0;
            for (int i = 1; i < orderedMappedRatings.Count; i++)
            {
                remainderVal += orderedMappedRatings[i].Value;
            }
            List<int> distroValues = new List<int>();
            distroValues.Add(remainderVal);
            for (int i = 1; i < orderedMappedRatings.Count; i++)
            {
                distroValues.Add(distroMaxVal);
            }
            List<KeyValuePair<Tendencies, int>> competitorDistroRatings = distroTendencies.Zip(distroValues, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v).ToList();
            //print("COMPETITOR DISTRIBUTION!");
            //print(String.Join(", ", competitorDistroRatings));

            List<Tendencies> tendencyList = new List<Tendencies>();
            foreach (var rating in competitorDistroRatings)
            {
                for (int i = 0; i < rating.Value; i++)
                {
                    tendencyList.Add(rating.Key);
                }
            }

            // spawn objects based on above.
            for (int i = 0; i < listOfTendencyPlacements.tendencyPlacements.Count; i++)
            {
                spawnObject(
                    listOfTendencyPlacements.tendencyPlacements[i].tendencyObjects.Find(obj => obj.GetComponent<FocusMeasures>().tendency.Equals(tendencyList[i])),
                    listOfTendencyPlacements.tendencyPlacements[i].placementPoint
                    );
            }

            yield return null;
        }

        private IEnumerator PresetLoader()
        {
            foreach (var tendencyPlacement in listOfTendencyPlacements.tendencyPlacements)
            {
                spawnObject(
                    tendencyPlacement.tendencyObjects.Find(obj => obj.GetComponent<FocusMeasures>().tendency.Equals(presetTendency)),
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