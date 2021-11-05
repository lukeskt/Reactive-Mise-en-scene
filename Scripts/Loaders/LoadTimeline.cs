using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ReactiveMiseEnScene
{
    [RequireComponent(typeof(PlayableDirector))]
    public class LoadTimeline : MonoBehaviour
    {
        public ReactiveMesSettings RMSettings;
        public ReactiveMesSettings.SingleResultTendencyAlgorithm algorithm;
        public ReactiveMesSettings.RequestType requestType;
        //public string presetTendency;
        //[HideInInspector] public int tendencyIndex = 0; // for custom editor
        public string localeRequest;
        [HideInInspector] public int localeIndex = 0; // for custom editor

        [Tooltip("WARNING EXPERIMENTAL FEATURE!\nIf Continuous is checked, update the tendency-based timeline every frame.")]
        public bool continuous = false;

        private PlayableDirector timelineDirector;
        public List<TimelineAsset> timelines;
        public TimelineAsset presetTimeline;
        private ReactiveMesDataManager DataMgr;

        public float timeToWait;

        // Start is called before the first frame update
        void Start()
        {
            timelineDirector = GetComponent<PlayableDirector>();
            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
            TimelineLoader(localeRequest);
        }

        private void Update()
        {
            if (continuous)
            {
                TimelineLoader(localeRequest); // change this to the local locale?
            }
        }

        private void TimelineLoader (string localeToRequest)
        {

            // Make sure timeline stopped before loading new timeline.
            timelineDirector.Stop();

            Dictionary<string, double> TendenciesFromDataMgr = new Dictionary<string, double>();
            string TendencyForTimeline;
            switch (requestType)
            {
                case ReactiveMesSettings.RequestType.Global:
                    TendenciesFromDataMgr = DataMgr.GetGlobalTendency(DataMgr.reactiveObjects);
                    break;
                case ReactiveMesSettings.RequestType.Locale:
                    TendenciesFromDataMgr = DataMgr.GetLocaleTendency(DataMgr.reactiveObjects, localeToRequest);
                    break;
                default:

                    break;
            }

            switch (algorithm)
            {
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.StrongestTendency:
                    TendencyForTimeline = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    timelineDirector.playableAsset = timelines.Find(profile => profile.name.Contains(TendencyForTimeline));
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.SecondStrongest:
                    var SortedTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).Reverse().ToList();
                    timelineDirector.playableAsset = timelines.Find(profile => profile.name.Contains(SortedTendencies[1].Key));
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.SecondWeakest:
                    var UnsortedTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).ToList();
                    timelineDirector.playableAsset = timelines.Find(profile => profile.name.Contains(UnsortedTendencies[1].Key));
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.WeakestTendency:
                    TendencyForTimeline = TendenciesFromDataMgr.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    timelineDirector.playableAsset = timelines.Find(profile => profile.name.Contains(TendencyForTimeline));
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.Random:
                    timelineDirector.playableAsset = timelines[Random.Range(0, timelines.Count)];
                    break;
                default:
                    goto case ReactiveMesSettings.SingleResultTendencyAlgorithm.StrongestTendency;
            }
        }
    }
}