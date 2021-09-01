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
        public ReactiveMesSettings.TendencyAlgorithm algorithm;
        public ReactiveMesSettings.RequestType requestType;
        public string presetTendency;
        [HideInInspector] public int localeIndex = 0; // for custom editor
        public string localeRequest;
        [HideInInspector] public int tendencyIndex = 0; // for custom editor

        private PlayableDirector timelineDirector;
        public List<TimelineAsset> timelines;
        public TimelineAsset presetTimeline;
        private ReactiveMesDataManager DataMgr;

        // times if we need them
        private float startTime;
        public float timeToWait;

        // Start is called before the first frame update
        void Start()
        {
            timelineDirector = GetComponent<PlayableDirector>();
            // Make sure timeline stopped before loading new timeline.
            timelineDirector.Stop();
            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
            
            startTime = Time.time;

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
                case ReactiveMesSettings.TendencyAlgorithm.MaxValue:
                    TendencyForTimeline = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    timelineDirector.playableAsset = timelines.Find(profile => profile.name.Contains(TendencyForTimeline.ToString()));
                    break;
                case ReactiveMesSettings.TendencyAlgorithm.MinValue:
                    TendencyForTimeline = TendenciesFromDataMgr.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    timelineDirector.playableAsset = timelines.Find(profile => profile.name.Contains(TendencyForTimeline.ToString()));
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
                    timelineDirector.playableAsset = presetTimeline;
                    break;
                case ReactiveMesSettings.TendencyAlgorithm.Random:
                    timelineDirector.playableAsset = timelines[Random.Range(0, timelines.Count)];
                    break;
                default:
                    goto case ReactiveMesSettings.TendencyAlgorithm.Preset;
            }
            // Start timeline after selecting one based on algorithm.
            timelineDirector.Play();
        }

        private bool CheckTimeSinceStart(float timeToWait)
        {
            float nowTime = Time.time;
            if (nowTime - startTime >= timeToWait)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void NotImpl()
        {
            throw new System.NotImplementedException();
        }
    }
}