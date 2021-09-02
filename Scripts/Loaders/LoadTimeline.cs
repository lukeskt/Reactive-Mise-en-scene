using System.Collections;
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
        public string presetTendency;
        [HideInInspector] public int localeIndex = 0; // for custom editor
        public string localeRequest;
        [HideInInspector] public int tendencyIndex = 0; // for custom editor

        private PlayableDirector timelineDirector;
        public List<TimelineAsset> timelines;
        public TimelineAsset presetTimeline;
        private ReactiveMesDataManager DataMgr;

        public float timeToWait;

        // Start is called before the first frame update
        void Start()
        {
            timelineDirector = GetComponent<PlayableDirector>();
            // Make sure timeline stopped before loading new timeline.
            timelineDirector.Stop();
            DataMgr = FindObjectOfType<ReactiveMesDataManager>();

            Dictionary<string, double> TendenciesFromDataMgr = new Dictionary<string, double>();
            string TendencyForTimeline;
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

            switch (algorithm)
            {
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.MaxValue:
                    TendencyForTimeline = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    timelineDirector.playableAsset = timelines.Find(profile => profile.name.Contains(TendencyForTimeline.ToString()));
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.MinValue:
                    TendencyForTimeline = TendenciesFromDataMgr.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    timelineDirector.playableAsset = timelines.Find(profile => profile.name.Contains(TendencyForTimeline.ToString()));
                    break;
                //case ReactiveMesSettings.SingleResultTendencyAlgorithm.Preset:
                //    timelineDirector.playableAsset = presetTimeline;
                //    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.Random:
                    timelineDirector.playableAsset = timelines[Random.Range(0, timelines.Count)];
                    break;
                default:
                    goto case ReactiveMesSettings.SingleResultTendencyAlgorithm.MaxValue;
            }
            // Start timeline after selecting one based on algorithm.
            timelineDirector.Play();
        }
    }
}