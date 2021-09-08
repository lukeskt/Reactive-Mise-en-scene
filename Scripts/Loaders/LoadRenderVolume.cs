using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace ReactiveMiseEnScene
{
    [RequireComponent(typeof(Volume), (typeof(Collider)))]
    public class LoadRenderVolume : MonoBehaviour
    {
        public ReactiveMesSettings RMSettings;
        public ReactiveMesSettings.SingleResultTendencyAlgorithm algorithm;
        public ReactiveMesSettings.RequestType requestType;
        //public string presetTendency;
        //[HideInInspector] public int tendencyIndex = 0; // for custom editor
        public string localeRequest;
        [HideInInspector] public int localeIndex = 0; // for custom editor

        private Volume volume;
        public List<VolumeProfile> volumeProfiles;
        public VolumeProfile presetVolumeProfile;
        private ReactiveMesDataManager DataMgr;

        // Start is called before the first frame update
        void Start()
        {
            volume = GetComponent<Volume>();

            DataMgr = FindObjectOfType<ReactiveMesDataManager>();
            Dictionary<string, double> TendenciesFromDataMgr = new Dictionary<string, double>();
            string TendencyForVolProfile;
            switch (requestType)
            {
                // note here: these are max-value derived, so akin to first-past the post... maybe check a threshold?
                // also offer an inversion / min value?
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
                    TendencyForVolProfile = TendenciesFromDataMgr.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
                    volume.profile = volumeProfiles.Find(profile => profile.name.Contains(TendencyForVolProfile));
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.RunnerUp:
                    var SortedTendencies = TendenciesFromDataMgr.ToList().OrderBy(x => x.Value).Reverse().ToList();
                    volume.profile = volumeProfiles.Find(profile => profile.name.Contains(SortedTendencies[1].Key));
                    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.MinValue:
                    TendencyForVolProfile = TendenciesFromDataMgr.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
                    volume.profile = volumeProfiles.Find(profile => profile.name.Contains(TendencyForVolProfile));
                    break;
                //case ReactiveMesSettings.SingleResultTendencyAlgorithm.Preset:
                //    volume.profile = presetVolumeProfile;
                //    break;
                case ReactiveMesSettings.SingleResultTendencyAlgorithm.Random:
                    volume.profile = volumeProfiles[Random.Range(0, volumeProfiles.Count)];
                    break;
                default:
                    goto case ReactiveMesSettings.SingleResultTendencyAlgorithm.MaxValue;
            }
        }
    }
}
